using DiplomaProject.Application.Exceptions;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class DeleteUserCommand(string id) : BaseCommand<bool>
{
    public string Id { get; set; } = id;

    public class DeleteUserCommandHandler(IMapper mapper, ICurrentUser currentUser, UserManager<User> userManager)
        : BaseCommandHandler<DeleteUserCommand>(mapper, currentUser)
    {
        public override async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            throw new BadRequestException(result.Errors.First().Description);
        }
    }
}