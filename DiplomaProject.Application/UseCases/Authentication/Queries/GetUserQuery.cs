using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.Exceptions;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Application.UseCases.Authentication.Queries;

public class GetUserQuery : BaseQuery<UserDTO>
{
    public GetUserQuery()
    {
    }
    
    public class GetUserQueryHandler : BaseQueryHandler<GetUserQuery>
    {
        private readonly UserManager<User> _userManager;
        
        public GetUserQueryHandler(IMapper mapper, IIdentityUserService identityUser, UserManager<User> userManager) 
            : base(mapper, identityUser)
        {
            _userManager = userManager;
        }

        public override async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(IdentityUserService.Id);
            if (user is null)
            {
                Console.WriteLine("User not found");
                throw new NotFoundException("User not found");
            }

            return Mapper.Map<UserDTO>(user);
        }
    }
}