using System.Security.Claims;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Http;

namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class IdentityUserService : IIdentityUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Id => _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    public string FirstName => _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? string.Empty;
    public string LastName => _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value ?? string.Empty;
    public string Email => _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "someTestEmail";
}