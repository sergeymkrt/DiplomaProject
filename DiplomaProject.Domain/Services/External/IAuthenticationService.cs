using DiplomaProject.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Domain.Services.External;

public interface IAuthenticationService
{
    Task<string> GenerateToken(User user);
    Task<(bool, IEnumerable<IdentityError>)> VerifyEmail(string token, string email);
}