using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Infrastructure.Persistence.DbContexts;

public class AppQueryContext : ReadableDbContext
{
    public DbSet<File> Files { get; set; }

    public AppQueryContext(DbContextOptions<AppQueryContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}