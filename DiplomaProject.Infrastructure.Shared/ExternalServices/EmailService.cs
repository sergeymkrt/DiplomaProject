using Azure;
using Azure.Communication.Email;
using Azure.Identity;
using DiplomaProject.Infrastructure.Shared.Configs;
using Microsoft.Extensions.Options;

namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class EmailService(IOptions<EmailServiceConfig> emailConfigs) : IEmailService
{
    private readonly EmailServiceConfig _emailConfigs = new()
    {
        ClientId = Environment.GetEnvironmentVariable("CLIENT_ID") ?? emailConfigs.Value.ClientId,
        ClientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? emailConfigs.Value.ClientSecret,
        TenantId = Environment.GetEnvironmentVariable("TENANT_ID") ?? emailConfigs.Value.TenantId,
        Endpoint = Environment.GetEnvironmentVariable("EMAIL_ENDPOINT") ?? emailConfigs.Value.Endpoint,
        VerificationLink = Environment.GetEnvironmentVariable("VERIFICATION_LINK") ?? emailConfigs.Value.VerificationLink,
        From = Environment.GetEnvironmentVariable("EMAIL_FROM") ?? emailConfigs.Value.From
    };

    public async Task SendVerificationEmail(string email, string token)
    {
        var client = GetEmailClient();

        var emailRecipients = new EmailRecipients(
        [
            new EmailAddress(email)
        ]);
        EmailContent emailContent = new("Verify Email")
        {
            Html = $"<html><body><p>Click <a href='{_emailConfigs.VerificationLink}{token}&email={email}'>here</a> to verify your email</p></body></html>"
        };

        EmailMessage emailMessage = new(_emailConfigs.From, emailRecipients, emailContent);

        var result = await client.SendAsync(WaitUntil.Completed, emailMessage);
        if (result.Value.Status != EmailSendStatus.Succeeded)
        {
            throw new Exception("Email not sent");
        }
    }

    public Task SendPasswordResetEmail(string email, string token)
    {
        var client = GetEmailClient();

        var emailRecipients = new EmailRecipients(
                   [
            new EmailAddress(email)
        ]);
        EmailContent emailContent = new("Reset Password");
        emailContent.Html = $"<html><body><p>Click <a href='{_emailConfigs.VerificationLink}{token}'>here</a> to reset your password</p></body></html>";


        EmailMessage emailMessage = new(_emailConfigs.From, emailRecipients, emailContent);

        return client.SendAsync(WaitUntil.Completed, emailMessage);
    }

    public Task SendEmail(string email, string subject, string message)
    {
        var client = GetEmailClient();
        var emailRecipients = new EmailRecipients(
            [
            new EmailAddress(email)
        ]);
        EmailContent emailContent = new(subject)
        {
            Html = message
        };

        EmailMessage emailMessage = new(_emailConfigs.From, emailRecipients, emailContent);

        return client.SendAsync(WaitUntil.Completed, emailMessage);
    }

    private EmailClient GetEmailClient()
    {
        var credential = new ChainedTokenCredential(new ClientSecretCredential(
            _emailConfigs.TenantId,
            _emailConfigs.ClientId,
            _emailConfigs.ClientSecret), new ManagedIdentityCredential());

        var client = new EmailClient(new Uri(_emailConfigs.Endpoint), credential);

        return client;
    }
}