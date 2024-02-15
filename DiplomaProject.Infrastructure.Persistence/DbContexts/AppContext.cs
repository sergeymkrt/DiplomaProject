using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Infrastructure.Persistence.DbContexts;

public class AppContext : WritableDbContext
{
    public DbSet<File> Files { get; set; }

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