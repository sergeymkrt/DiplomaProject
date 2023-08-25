using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.Exceptions;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;
using Role = DiplomaProject.Domain.Enums.Role;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class RegisterUserCommand : BaseCommand<bool>
{
    public RegisterUserCommand(RegisterUserDTO dto)
    {
        DTO = dto;
    }

    public RegisterUserDTO DTO { get; set; }
    
    public class RegisterUserCommandHandler : BaseCommandHandler<RegisterUserCommand>
    {
        private readonly UserManager<User> _userManager;
        
        public RegisterUserCommandHandler(IMapper mapper, IIdentityUserService identityUser, UserManager<User> userManager) 
            : base(mapper, identityUser)
        {
            _userManager = userManager;
        }

        public override async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = Mapper.Map<User>(request.DTO);
            var result = await _userManager.CreateAsync(user, request.DTO.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user,new List<string> { Role.USER.ToString("F")} );
                return true;
            }
            throw new BadRequestException(result.Errors.First().Description);
        }
    }
}