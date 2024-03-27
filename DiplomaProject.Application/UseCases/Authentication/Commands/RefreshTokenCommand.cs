using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Models;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class RefreshTokenCommand(string refreshToken) : BaseCommand<TokenModel>
{
    public string RefreshToken { get; set; } = refreshToken;

    public class RefreshTokenCommandHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IAuthenticationService authenticationService)
        : BaseCommandHandler<RefreshTokenCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel<TokenModel>> Handle(RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var userId = await authenticationService.GetIdFromRefreshTokenAsync(request.RefreshToken);
            if (userId == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var user = await CurrentUser.GetUserWithRelations(userId, true);
            if (user is null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            var session = user.UserSessions.FirstOrDefault(x => x.RefreshToken == request.RefreshToken);

            if (session is null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            if (session.IsExpired)
            {
                throw new UnauthorizedAccessException("Refresh token expired");
            }

            var tokenModel = await authenticationService.GenerateToken(user);


            session.RefreshToken = tokenModel.RefreshToken;
            session.ExpirationDate = DateTimeOffset.Now.AddSeconds(tokenModel.RefreshTokenExpiresIn);

            return ResponseModel<TokenModel>.Create(tokenModel);
        }
    }
}