using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Exceptions;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Application.UseCases.Authentication.Queries;

public class GetUserQuery : BaseQuery<UserDto>
{
    public class GetUserQueryHandler(IMapper mapper, ICurrentUser currentUser, UserManager<User> userManager)
        : BaseQueryHandler<GetUserQuery>(mapper, currentUser)
    {
        public override async Task<ResponseModel<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(CurrentUser.Id);
            if (user is null)
            {
                Console.WriteLine("User not found");
                throw new NotFoundException("User not found");
            }

            return ResponseModel<UserDto>.Create(ResponseCode.Success, Mapper.Map<UserDto>(user));
        }
    }
}