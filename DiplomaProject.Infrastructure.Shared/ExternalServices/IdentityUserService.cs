namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string Id => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "Unknown";
    public string FirstName => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? "Unknown";
    public string LastName => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value ?? "Unknown";
    public string Email => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "Unknown";
}