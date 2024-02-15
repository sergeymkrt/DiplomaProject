namespace DiplomaProject.Application.DTOs.Authentication;

public class RegisterUserDTO(string name, string surName, string userName, string phoneNumber, string email)
    : UserDto(name, surName, userName, phoneNumber, email)
{
    public string Password { get; set; }
}