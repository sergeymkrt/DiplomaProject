using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using DiplomaProject.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Name ?? String.Empty),
            new(ClaimTypes.Surname, user.SurName ?? String.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        
        var securityKey = new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP521));
        securityKey.ECDsa.ImportFromPem(_configuration["JWT:PrivateKey"]);
        
        var accessTokenHours = int.Parse(_configuration["JWT:AccessTokenValidityInHours"]);
        
        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(accessTokenHours),
            claims: authClaims,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha512)
        );
        var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenstring;
    }
}