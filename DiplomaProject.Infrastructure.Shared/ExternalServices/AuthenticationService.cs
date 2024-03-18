using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Infrastructure.Shared.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class AuthenticationService(UserManager<User> userManager, IOptions<JwtConfig> config) : IAuthenticationService
{
    public async Task<string> GenerateToken(User user)
    {
        var userRoles = await userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Name ?? string.Empty),
            new(ClaimTypes.Surname, user.SurName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var securityKey = new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP521));
        securityKey.ECDsa.ImportFromPem(config.Value.PrivateKey);

        var token = new JwtSecurityToken(
            config.Value.ValidIssuer,
            config.Value.ValidAudience,
            expires: DateTime.Now.AddHours(config.Value.AccessTokenValidityInHours),
            claims: authClaims,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha512)
        );
        var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenstring;
    }

    public async Task<(bool, IEnumerable<IdentityError>)> VerifyEmail(string token, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (false, []);
        }

        var result = await userManager.ConfirmEmailAsync(user, token);

        return (result.Succeeded, result.Errors);
    }
}