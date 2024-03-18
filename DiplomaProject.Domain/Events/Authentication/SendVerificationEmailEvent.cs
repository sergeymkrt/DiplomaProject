namespace DiplomaProject.Domain.Events.Authentication;

public class SendVerificationEmailEvent(string email) : DomainEvent
{
    public string Email { get; set; } = email;
}