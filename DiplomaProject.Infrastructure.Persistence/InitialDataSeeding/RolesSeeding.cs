using DiplomaProject.Domain.Entities.User;
using DomainEnums = DiplomaProject.Domain.Enums;

namespace DiplomaProject.Infrastructure.Persistence.InitialDataSeeding;

public static class RolesSeeding
{
    internal static async Task<AppContext> SeedRolesAsync(
        this AppContext context)
    {
        if (context.Roles.Any())
            return context;

        var roleNames = Enum.GetValues(typeof(DomainEnums.Role))
            .Cast<DomainEnums.Role>()
            .ToDictionary(role => role, role => role.ToString("F"));

        await context.Roles.AddRangeAsync(new List<Role>()
        {
            new() {Name = roleNames[DomainEnums.Role.ADMIN],NormalizedName = roleNames[DomainEnums.Role.ADMIN].ToUpper()},
            new() {Name = roleNames[DomainEnums.Role.USER],NormalizedName = roleNames[DomainEnums.Role.USER].ToUpper()}
        });
        await context.SaveChangesAsync();
        return context;
    }
}