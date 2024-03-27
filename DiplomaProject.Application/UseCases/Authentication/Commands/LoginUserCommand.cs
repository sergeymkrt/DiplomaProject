using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Exceptions;
using DiplomaProject.Domain.Models;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class LoginUserCommand(AuthUserDto dto) : BaseCommand<TokenModel>
{
    public AuthUserDto DTO { get; set; } = dto;

    public class LoginUserCommandHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        UserManager<User> userManager,
        IAuthenticationService authenticationService)
        : BaseCommandHandler<LoginUserCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel<TokenModel>> Handle(LoginUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.DTO.UserName);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            if (await userManager.CheckPasswordAsync(user, request.DTO.Password))
            {
                var tokenModel = await authenticationService.GenerateToken(user);

                await currentUser.UpdateCurrentUserContext(user);
                user = await currentUser.GetUserWithRelations(isTracking: true);

                var session = user.UserSessions.FirstOrDefault(x => x.RefreshToken == tokenModel.RefreshToken);
                if (session != null)
                {
                    session.RefreshToken = tokenModel.RefreshToken;
                    session.ExpirationDate = DateTimeOffset.Now.AddSeconds(tokenModel.RefreshTokenExpiresIn);
                    session.LastLogin = DateTimeOffset.Now;
                }
                else
                {
                    var expiresIn = DateTimeOffset.Now.AddSeconds(tokenModel.RefreshTokenExpiresIn);
                    var currentUserSession = await CurrentUser.GetCurrentUserSession(tokenModel.RefreshToken, expiresIn);
                    user.UserSessions.Add(currentUserSession);
                }

                return ResponseModel<TokenModel>.Create(tokenModel);
            }

            throw new UnauthorizedAccessException("Invalid password");
        }
    }
}