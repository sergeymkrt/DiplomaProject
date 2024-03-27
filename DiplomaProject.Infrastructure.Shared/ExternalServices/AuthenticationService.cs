using DiplomaProject.Domain.AggregatesModel.BlackLists;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Models;
using DiplomaProject.Infrastructure.Shared.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class AuthenticationService(IBlackListRepository blackListRepository, UserManager<User> userManager, IOptions<JwtConfig> config) : IAuthenticationService
{
    public Task<bool> IsTokenBlackListedAsync(string token)
    {
        return blackListRepository.AnyAsync(x => x.Token == token);
    }

    public async Task<TokenModel> GenerateToken(User user)
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
            expires: DateTime.Now.AddMinutes(config.Value.AccessTokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha512)
        );

        var refreshToken = new JwtSecurityToken(
            config.Value.ValidIssuer,
            config.Value.ValidAudience,
            expires: DateTime.Now.AddDays(config.Value.RefreshTokenValidityInDays),
            claims: authClaims,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha512)
        );

        var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);
        return new TokenModel
        {
            AccessToken = tokenstring,
            RefreshToken = refreshTokenString,
            AccessTokenExpiresIn = config.Value.AccessTokenValidityInMinutes * 60,
            RefreshTokenExpiresIn = config.Value.RefreshTokenValidityInDays * 24 * 60 * 60
        };
    }

    public Task<string?> GetIdFromRefreshTokenAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var tokenS = handler.ReadJwtToken(token);

        return Task.FromResult(tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);

        // return handler.ReadToken(token) is JwtSecurityToken tokenS ? Task.FromResult(tokenS.ValidTo > DateTime.Now) : Task.FromResult(false);
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