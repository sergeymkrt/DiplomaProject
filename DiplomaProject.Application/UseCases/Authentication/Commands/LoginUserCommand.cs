using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.Exceptions;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class LoginUserCommand : BaseCommand<string>
{
    public AuthUserDTO DTO { get; set; }

    public LoginUserCommand(AuthUserDTO dto)
    {
        DTO = dto;
    }

    public class LoginUserCommandHandler : BaseCommandHandler<LoginUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;
        
        public LoginUserCommandHandler(IMapper mapper, IIdentityUserService identityUser, UserManager<User> userManager,
            IAuthenticationService authenticationService) 
            : base(mapper, identityUser)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        public override async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.DTO.UserName);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            if (await _userManager.CheckPasswordAsync(user, request.DTO.Password))
            {
                return await _authenticationService.GenerateToken(user);
            }
            else
            {
                throw new BadRequestException("Invalid password");
            }
        }
    }
}