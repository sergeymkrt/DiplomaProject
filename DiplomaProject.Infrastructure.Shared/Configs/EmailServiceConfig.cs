namespace DiplomaProject.Infrastructure.Shared.Configs;

public class EmailServiceConfig
{
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Endpoint { get; set; }
    public string From { get; set; }
    public string VerificationLink { get; set; }
}