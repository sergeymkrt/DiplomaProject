using DiplomaProject.Domain.AggregatesModel.BlackLists;
using DiplomaProject.Domain.AggregatesModel.Groups;
using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Shared.Lookups;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Infrastructure.Persistence.DbContexts;

public class AppContext : IdentityDbContext<User, Role, string>, IUnitOfWork
{
    private readonly IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;

    #region Aggregates
    public DbSet<File> Files { get; set; }
    public DbSet<Key> Keys { get; set; }
    public DbSet<Directory> Directories { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }

    public DbSet<BlackList> BlackLists { get; set; }
    #endregion

    #region Lookups
    public DbSet<KeySize> KeySizes { get; set; }
    public DbSet<PermissionType> PermissionTypes { get; set; }
    public DbSet<AccessLevel> AccessLevels { get; set; }
    #endregion

    public AppContext(DbContextOptions<AppContext> options) : base(options) { }

    public AppContext(
        DbContextOptions<AppContext> options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Ignore<DomainEvent>();
    }

    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, string userId)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents is { Count: > 0 })
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            var preDomainEvents = domainEvents.Where(x => !x.IsPostEvent).ToList();
            var postDomainEvents = domainEvents.Where(x => x.IsPostEvent).ToList();

            await _mediator.DispatchDomainEventsAsync(preDomainEvents);

            await SaveChangesAsync(userId);

            await transaction.CommitAsync();
            try
            {
                await _mediator.DispatchDomainEventsAsync(postDomainEvents);
            }
            catch
            {
                // pass
                // post domain events should not affect on transaction
            }

        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task<int> SaveChangesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var addedEntries = ChangeTracker.Entries().Where(p => p.State == EntityState.Added).ToList();
        var modifiedEntries = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();

        SetCreators(addedEntries, userId);
        SetModifiers(modifiedEntries, userId);

        var res = await base.SaveChangesAsync(cancellationToken);

        return res;
    }

    private static void SetCreators(List<EntityEntry> entityEntries, string currentUserId)
    {
        if (string.IsNullOrWhiteSpace(currentUserId))
            return;

        var newEntries = entityEntries.FindAll(x => x.Entity.GetType().GetInterfaces().Contains(typeof(ICreator)));
        if (newEntries.Any())
        {
            newEntries.ForEach(e => (e.Entity as ICreator)?.SetCreator(currentUserId, DateTimeOffset.UtcNow));
        }
    }

    private static void SetModifiers(List<EntityEntry> entityEntries, string currentUserId)
    {
        if (string.IsNullOrWhiteSpace(currentUserId))
            return;

        var modifiedEntries = entityEntries.FindAll(x => x.Entity.GetType().GetInterfaces().Contains(typeof(IModifier)));
        if (modifiedEntries.Any())
        {
            modifiedEntries.ForEach(e => (e.Entity as IModifier)?.SetModifier(currentUserId, DateTimeOffset.UtcNow));
        }
    }

    private List<DomainEvent> GetDomainEvents(bool onlyPre = true)
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = onlyPre
            ? domainEntities.SelectMany(x => x.Entity.DomainEvents).Where(x => !x.IsPostEvent)
            : domainEntities.SelectMany(x => x.Entity.DomainEvents).Where(x => x.IsPostEvent);

        return domainEvents.ToList();
    }
}