using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Groups;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Groups.Commands;

public class DeleteUserFromGroupCommand(long groupId, string userId) : BaseCommand
{
    public long GroupId { get; set; } = groupId;
    public string UserId { get; set; } = userId;

    public class DeleteUserFromGroupCommandHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IGroupDomainService groupDomainService)
        : BaseCommandHandler<DeleteUserFromGroupCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel> Handle(DeleteUserFromGroupCommand request,
            CancellationToken cancellationToken)
        {
            await groupDomainService.RemoveUserFromGroup(request.UserId, request.GroupId);
            return ResponseModel.Create(ResponseCode.SuccessfullyDeleted);
        }
    }
}