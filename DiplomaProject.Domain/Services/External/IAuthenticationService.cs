using DiplomaProject.Domain.Entities.User;

namespace DiplomaProject.Domain.Services.External;

public interface IAuthenticationService
{
    Task<string> GenerateToken(User user);
}