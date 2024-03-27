using DiplomaProject.Application.Models;
using DiplomaProject.Domain.AggregatesModel.BlackLists;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class DeleteUserSessionCommand(Guid id) : BaseCommand
{
    public Guid Id { get; set; } = id;

    public class DeleteUserSessionCommandHandler(
        ICurrentUser currentUser,
        IBlackListRepository blackListRepository,
        IMapper mapper)
        : BaseCommandHandler<DeleteUserSessionCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel> Handle(DeleteUserSessionCommand request, CancellationToken cancellationToken)
        {
            var user = await currentUser.GetUserWithRelations(currentUser.Id, true);
            if (user is null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            var session = user.UserSessions.FirstOrDefault(x => x.Id == request.Id);
            if (session is null)
            {
                throw new UnauthorizedAccessException("Session not found");
            }

            user.UserSessions.Remove(session);

            await blackListRepository.AddAsync(new BlackList
            {
                Token = session.RefreshToken,
                ExpirationDate = session.ExpirationDate
            });

            return ResponseModel.Create(ResponseCode.SuccessfullyDeleted);
        }
    }
}