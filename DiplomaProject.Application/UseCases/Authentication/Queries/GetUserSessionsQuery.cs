using DiplomaProject.Application.Models;
using DiplomaProject.Domain.AggregatesModel.UserSessions;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Authentication.Queries;

public class GetUserSessionsQuery : BaseQuery<List<UserSession>>
{
    public class GetUserSessionsQueryHandler(
        ICurrentUser currentUser,
        IMapper mapper)
        : BaseQueryHandler<GetUserSessionsQuery>(mapper, currentUser)
    {
        public override async Task<ResponseModel<List<UserSession>>> Handle(GetUserSessionsQuery request,
            CancellationToken cancellationToken)
        {
            var user = await CurrentUser.GetUserWithRelations(CurrentUser.Id, true);
            if (user is null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            return ResponseModel<List<UserSession>>.Create(user.UserSessions.ToList());
        }
    }
}