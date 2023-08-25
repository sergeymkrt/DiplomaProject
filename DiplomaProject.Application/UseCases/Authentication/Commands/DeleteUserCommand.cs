using DiplomaProject.Application.Exceptions;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class DeleteUserCommand : BaseCommand<bool>
{
    public string Id { get; set; }
    
    public DeleteUserCommand(string id)
    {
        Id = id;
    }
    
    public class DeleteUserCommandHandler : BaseCommandHandler<DeleteUserCommand>
    {
        private readonly UserManager<User> _userManager;
        
        public DeleteUserCommandHandler(IMapper mapper, IIdentityUserService identityUser, UserManager<User> userManager) 
            : base(mapper, identityUser)
        {
            _userManager = userManager;
        }

        public override async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            throw new BadRequestException(result.Errors.First().Description);
        }
    }
}