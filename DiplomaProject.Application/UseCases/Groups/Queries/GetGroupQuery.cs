using DiplomaProject.Application.DTOs.Groups;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Groups;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Groups.Queries;

public class GetGroupQuery(long groupId) : BaseQuery<GetGroupViewDto>
{
    public long GroupId { get; set; } = groupId;

    public class GetGroupQueryHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IGroupDomainService groupDomainService)
        : BaseQueryHandler<GetGroupQuery>(mapper, currentUser)
    {
        public override async Task<ResponseModel<GetGroupViewDto>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await groupDomainService.GetGroup(request.GroupId);

            if (group == null)
            {
                return ResponseModel<GetGroupViewDto>.Create(ResponseCode.NotExists, data: null, "Group");
            }

            var groupDto = Mapper.Map<GetGroupViewDto>(group);
            return ResponseModel<GetGroupViewDto>.Create(ResponseCode.Success, data: groupDto);
        }
    }
}