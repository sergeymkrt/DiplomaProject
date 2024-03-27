using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Domain.Services.External;

public interface IAuthenticationService
{
    Task<bool> IsTokenBlackListedAsync(string token);
    Task<TokenModel> GenerateToken(User user);
    Task<string?> GetIdFromRefreshTokenAsync(string token);
    Task<(bool, IEnumerable<IdentityError>)> VerifyEmail(string token, string email);
}