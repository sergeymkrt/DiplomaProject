using DiplomaProject.Domain.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DiplomaProject.Infrastructure.Persistence.DbContexts;

public abstract class WritableDbContext : IdentityDbContext<User,Role,string>, IUnitOfWork
{
    private readonly IMediator _mediator;
    private IDbContextTransaction? _currentTransaction;
    private readonly IIdentityUserService _identityUser;

    protected WritableDbContext() { }

    protected WritableDbContext(DbContextOptions options) : base(options) { }

    protected WritableDbContext(
        DbContextOptions options,
        IMediator mediator,
        IIdentityUserService identityUser) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _identityUser = identityUser ?? throw new ArgumentNullException(nameof(identityUser));
    }

    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            var preDomainEvents = GetDomainEvents();
            var postDomainEvents = GetDomainEvents(onlyPre: false);

            await SaveChangesAsync();

            await _mediator.DispatchDomainEventsAsync(preDomainEvents);
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var addedEntries = ChangeTracker.Entries().Where(p => p.State == EntityState.Added).ToList();
        var modifiedEntries = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();

        SetCreators(addedEntries, _identityUser.Email);
        SetModifiers(modifiedEntries, _identityUser.Email);

        var res = await base.SaveChangesAsync(cancellationToken);

        return res;
    }

    private static void SetCreators(List<EntityEntry> entityEntries, string currentUserEmail)
    {
        if (string.IsNullOrWhiteSpace(currentUserEmail))
            return;

        var newEntries = entityEntries.FindAll(x => x.Entity.GetType().GetInterfaces().Contains(typeof(ICreator)));
        if (newEntries.Any())
        {
            newEntries.ForEach(e => (e.Entity as ICreator)?.SetCreator(currentUserEmail, DateTimeOffset.UtcNow));
        }
    }

    private static void SetModifiers(List<EntityEntry> entityEntries, string currentUserEmail)
    {
        if (string.IsNullOrWhiteSpace(currentUserEmail))
            return;

        var modifiedEntries = entityEntries.FindAll(x => x.Entity.GetType().GetInterfaces().Contains(typeof(IModifier)));
        if (modifiedEntries.Any())
        {
            modifiedEntries.ForEach(e => (e.Entity as IModifier)?.SetModifier(currentUserEmail, DateTimeOffset.UtcNow));
        }
    }

    private List<INotification> GetDomainEvents(bool onlyPre = true)
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = onlyPre
            ? domainEntities.SelectMany(x => x.Entity.DomainEvents).Where(x => x is PreDomainEvent)
            : domainEntities.SelectMany(x => x.Entity.DomainEvents).Where(x => x is PostDomainEvent);

        return domainEvents.ToList();
    }
}