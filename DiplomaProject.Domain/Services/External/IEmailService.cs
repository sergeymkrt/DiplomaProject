namespace DiplomaProject.Domain.Services.External;

public interface IEmailService
{
    Task SendVerificationEmail(string email, string token);
    Task SendPasswordResetEmail(string email, string token);
    Task SendEmail(string email, string subject, string message);
}