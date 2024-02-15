namespace DiplomaProject.Application.DTOs.Authentication;

public class UserDto(string name, string surName, string userName, string phoneNumber, string email)
{
    public string Name { get; set; } = name;
    public string SurName { get; set; } = surName;
    public string UserName { get; set; } = userName;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Email { get; set; } = email;
}