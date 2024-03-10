using DiplomaProject.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class CurrentUser(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) : ICurrentUser
{
    public string Id => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "Unknown";
    public string FirstName => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? "Unknown";
    public string LastName => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value ?? "Unknown";
    public string Email => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "Unknown";

    public Task<User?> GetUserWithGroups()
    {
        return userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == Id)
            .Include(x => x.UserGroups)
            .ThenInclude(x => x.Group)
            .FirstOrDefaultAsync(x => x.Id == Id);
    }
}