using DiplomaProject.Application.DTOs.Groups;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Groups;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Groups.Commands;

public class CreateGroupCommand(GroupDto dto) : BaseCommand
{
    public GroupDto Dto { get; set; } = dto;

    public class CreateGroupCommandHandler(
        ICurrentUser currentUser,
        IMapper mapper,
        IGroupDomainService groupDomainService)
        : BaseCommandHandler<CreateGroupCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel> Handle(CreateGroupCommand request,
            CancellationToken cancellationToken)
        {
            var user = await CurrentUser.GetUserWithGroups();
            await groupDomainService.CreateGroup(request.Dto.Name, request.Dto.Description, request.Dto.AccessLevelId, user);

            return ResponseModel.Create(ResponseCode.SuccessfullyCreated);
        }
    }
}