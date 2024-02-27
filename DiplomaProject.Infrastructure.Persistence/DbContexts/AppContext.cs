using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.Shared.Lookups;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Infrastructure.Persistence.DbContexts;

public class AppContext : WritableDbContext
{
    #region Aggregates
    public DbSet<File> Files { get; set; }
    public DbSet<Key> Keys { get; set; }
    #endregion

    #region Lookups
    public DbSet<KeySize> KeySizes { get; set; }
    #endregion

    public AppContext()
    {
    }
    public AppContext(DbContextOptions<AppContext> options) : base(options) { }

    public AppContext(
        DbContextOptions<AppContext> options,
        IMediator mediator,
        ICurrentUser currentUser) : base(options, mediator, currentUser) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}