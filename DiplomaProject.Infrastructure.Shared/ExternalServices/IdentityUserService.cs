using DiplomaProject.Domain.AggregatesModel.UserSessions;
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

    public Task<User?> GetUserWithRelations(string? id = null, bool isTracking = false)
    {
        var query = userManager.Users.AsQueryable();

        if (!isTracking)
        {
            query = query.AsNoTracking();
        }

        query = id != null ? query.Where(u => u.Id == id) : query.Where(u => u.Id == Id);


        return query
            .Include(x => x.UserGroups)
            .ThenInclude(x => x.Group)
            .Include(x => x.PersonalDirectory)
            .Include(x => x.AccessLevel)
            .Include(x => x.UserSessions)
            .FirstOrDefaultAsync();
    }

    public Task UpdateCurrentUserContext(User user)
    {
        httpContextAccessor.HttpContext?.User?.AddIdentity(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Name ?? string.Empty),
            new(ClaimTypes.Surname, user.SurName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        }));
        return Task.CompletedTask;
    }

    public Task<UserSession> GetCurrentUserSession(string refreshToken, DateTimeOffset expirationDate)
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
        var ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var userSession = new UserSession
        {
            RefreshToken = refreshToken,
            ExpirationDate = expirationDate,
            UserAgent = userAgent,
            IpAddress = ipAddress,
            LastLogin = DateTimeOffset.Now
        };
        return Task.FromResult(userSession);
    }
}