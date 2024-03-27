using DiplomaProject.Domain.Entities.User;

namespace DiplomaProject.Domain.AggregatesModel.UserSessions;

public class UserSession
{
    public Guid Id { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset LastLogin { get; set; }
    public string UserAgent { get; set; }
    public string IpAddress { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }

    public string UserId { get; set; }
    public virtual User User { get; set; }

    public bool IsExpired => ExpirationDate < DateTimeOffset.Now;
    public bool IsActive => !IsExpired;
}