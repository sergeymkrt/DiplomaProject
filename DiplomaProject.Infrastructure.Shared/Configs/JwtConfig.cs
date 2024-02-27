namespace DiplomaProject.Infrastructure.Shared.Configs;

public class JwtConfig
{
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public string PrivateKey { get; set; }
    public string RefreshTokenSecret { get; set; }
    public int RefreshTokenSecretExpirationMinutes { get; set; }
    public int RefreshTokenValidityInDays { get; set; }
    public int AccessTokenValidityInHours { get; set; }
}