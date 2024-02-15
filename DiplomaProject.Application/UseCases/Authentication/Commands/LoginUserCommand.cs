using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.Exceptions;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class LoginUserCommand(AuthUserDto dto) : BaseCommand<string>
{
    public AuthUserDto DTO { get; set; } = dto;

    public class LoginUserCommandHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        UserManager<User> userManager,
        IAuthenticationService authenticationService)
        : BaseCommandHandler<LoginUserCommand>(mapper, currentUser)
    {
        public override async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.DTO.UserName);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            if (await userManager.CheckPasswordAsync(user, request.DTO.Password))
            {
                return await authenticationService.GenerateToken(user);
            }
            else
            {
                throw new BadRequestException("Invalid password");
            }
        }
    }
}