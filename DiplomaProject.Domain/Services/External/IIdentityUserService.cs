namespace DiplomaProject.Domain.Services.External;

public interface ICurrentUser
{
    string Id { get; }
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
}