namespace DiplomaProject.Application.DTOs.Authentication;

public class AuthUserDto(string userName, string password)
{
    public string UserName { get; set; } = userName;
    public string Password { get; set; } = password;
}