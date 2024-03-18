using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.Exceptions;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Events.Authentication;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;
using Role = DiplomaProject.Domain.Enums.Role;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class RegisterUserCommand(RegisterUserDTO dto) : BaseCommand<bool>
{
    public RegisterUserDTO DTO { get; set; } = dto;

    public class RegisterUserCommandHandler(IMapper mapper, IMediator mediator, ICurrentUser currentUser, UserManager<User> userManager)
        : BaseCommandHandler<RegisterUserCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = Mapper.Map<User>(request.DTO);
            user.PersonalDirectory = new Directory("Personal");
            var result = await userManager.CreateAsync(user, request.DTO.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(user, [Role.USER.ToString("F")]);
                await mediator.Publish(new SendVerificationEmailEvent(user.Email), cancellationToken);
                return ResponseModel<bool>.Create(ResponseCode.Success, true);
            }
            throw new BadRequestException(result.Errors.First().Description);
        }
    }
}