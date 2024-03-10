using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Groups;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Groups.Commands;

public class AddUserToGroupCommand(long groupId, string userId, int permissionId) : BaseCommand
{
    public long GroupId { get; set; } = groupId;
    public string UserId { get; set; } = userId;
    public int PermissionId { get; set; } = permissionId;

    public class AddUserToGroupCommandHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IGroupDomainService groupDomainService)
        : BaseCommandHandler<AddUserToGroupCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel> Handle(AddUserToGroupCommand request,
            CancellationToken cancellationToken)
        {
            await groupDomainService.AddUserToGroup(request.UserId, request.GroupId, request.PermissionId);
            return ResponseModel.Create(ResponseCode.SuccessfullyCreated);
        }
    }

}