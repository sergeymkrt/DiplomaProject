using DiplomaProject.Application.Interfaces;
namespace DiplomaProject.Infrastructure.Persistence.InitialDataSeeding;

public class DbInitializer : IDbInitializer
{
    private readonly AppContext _context;
    public DbInitializer(AppContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // await _context.Database.MigrateAsync();

        await _context.SeedRolesAsync();
    }
}