namespace DiplomaProject.Domain.Models;

public class TokenModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int AccessTokenExpiresIn { get; set; }
    public int RefreshTokenExpiresIn { get; set; }
}