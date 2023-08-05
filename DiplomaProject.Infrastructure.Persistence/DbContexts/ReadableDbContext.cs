using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.Infrastructure.Persistence.DbContexts;

public abstract class ReadableDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    protected ReadableDbContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}