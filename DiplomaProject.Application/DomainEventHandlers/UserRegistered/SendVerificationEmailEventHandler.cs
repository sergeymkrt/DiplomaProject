using DiplomaProject.Application.Exceptions;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Events.Authentication;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Application.DomainEventHandlers.UserRegistered;

public class SendVerificationEmailEventHandler(UserManager<User> userManager, IEmailService emailService) : INotificationHandler<SendVerificationEmailEvent>
{
    public async Task Handle(SendVerificationEmailEvent notification, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(notification.Email) ?? throw new BadRequestException("User not found");
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await emailService.SendVerificationEmail(user.Email, token);
    }
}