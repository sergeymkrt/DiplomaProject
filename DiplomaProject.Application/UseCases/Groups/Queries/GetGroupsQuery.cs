using DiplomaProject.Application.DTOs.Groups;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.SeedWork;
using DiplomaProject.Domain.Services.DomainServices.Groups;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Groups.Queries;

public class GetGroupsQuery(int pageNumber,
    int pageSize,
    string search,
    string orderByColumn,
    bool isAsc) : BasePaginatedQuery<GetGroupViewDto>(pageNumber, pageSize, search, orderByColumn, isAsc)
{
    public class GetGroupsQueryHandler(
        ICurrentUser currentUser,
        IMapper mapper,
        IGroupDomainService groupDomainService)
        : BaseQueryHandler<GetGroupsQuery>(currentUser, mapper)
    {
        public override async Task<ResponseModel<Paginated<GetGroupViewDto>>> Handle(GetGroupsQuery request,
            CancellationToken cancellationToken)
        {
            var groups = await groupDomainService.GetGroups(
                               predicate: g => g.OwnerId == CurrentUser.Id
                                               || g.UserGroups.Any(ug => ug.UserId == CurrentUser.Id),
                                              pageNumber: request.PageNumber,
                                              pageSize: request.PageSize,
                                              search: request.Search,
                                              orderBy: request.OrderBy);
            var groupDtos = Mapper.Map<Paginated<GetGroupViewDto>>(groups);
            return ResponseModel<Paginated<GetGroupViewDto>>.Create(groupDtos);
        }
    }
}