namespace DiplomaProject.Domain.Services.External;

public interface IIdentityUserService
{
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
}