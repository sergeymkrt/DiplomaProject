using DiplomaProject.Domain.AggregatesModel.UserSessions;
using DiplomaProject.Domain.Entities.User;

namespace DiplomaProject.Domain.Services.External;

public interface ICurrentUser
{
    string Id { get; }
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }

    Task<User?> GetUserWithRelations(string? id = null, bool isTracking = false);
    Task UpdateCurrentUserContext(User user);
    Task<UserSession> GetCurrentUserSession(string refreshToken, DateTimeOffset expirationDate);
}