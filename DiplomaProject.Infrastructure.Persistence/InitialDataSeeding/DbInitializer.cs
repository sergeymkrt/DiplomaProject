using DiplomaProject.Application.Interfaces;
using DiplomaProject.Domain.Entities.User;
using DomainEnums = DiplomaProject.Domain.Enums;
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
        await _context.Database.MigrateAsync();

        if (_context.Roles.Any())
            return;
        
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        // collect the Role names from the DomainEnums.Role into a dictionary with Role value as key and Role name as value
        var roleNames = Enum.GetValues(typeof(DomainEnums.Role))
            .Cast<DomainEnums.Role>()
            .ToDictionary(role => role, role => role.ToString("F"));
        
        await _context.Roles.AddRangeAsync(new List<Role>()
        {
            new() {Name = roleNames[DomainEnums.Role.ADMIN],NormalizedName = roleNames[DomainEnums.Role.ADMIN].ToUpper()},
            new() {Name = roleNames[DomainEnums.Role.USER],NormalizedName = roleNames[DomainEnums.Role.USER].ToUpper()}
        });
        await _context.SaveChangesAsync();
        
        await transaction.CommitAsync();
    }
}