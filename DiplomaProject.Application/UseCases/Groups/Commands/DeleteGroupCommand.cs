using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Groups;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Groups.Commands;

public class DeleteGroupCommand(long groupId) : BaseCommand
{
    public long GroupId { get; set; } = groupId;

    public class DeleteGroupCommandHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IGroupDomainService groupDomainService)
        : BaseCommandHandler<DeleteGroupCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel> Handle(DeleteGroupCommand request,
            CancellationToken cancellationToken)
        {
            await groupDomainService.DeleteGroup(request.GroupId, UserId);
            return ResponseModel.Create(ResponseCode.SuccessfullyDeleted);
        }
    }
}