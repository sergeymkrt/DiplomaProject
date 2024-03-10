using DiplomaProject.Application.DTOs.Directories;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.SeedWork;
using DiplomaProject.Domain.Services.DomainServices.Directories;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Directories.Queries;

public class GetDirectoriesQuery(int pageNumber,
    int pageSize,
    string search,
    string orderByColumn,
    bool isAsc) : BasePaginatedQuery<DirectoryDto>(pageNumber, pageSize, search, orderByColumn, isAsc)
{
    public class GetDirectoriesQueryHandler(
        ICurrentUser currentUser,
        IMapper mapper,
        IDirectoryDomainService directoryDomainService)
        : BaseQueryHandler<GetDirectoriesQuery>(currentUser, mapper)
    {
        public override async Task<ResponseModel<Paginated<DirectoryDto>>> Handle(GetDirectoriesQuery request,
            CancellationToken cancellationToken)
        {
            var user = await CurrentUser.GetUserWithGroups();
            var userGroupIds = user.UserGroups.Select(g => g.GroupId).ToList();
            var directories = await directoryDomainService.GetDirectoriesAsync(
                               predicate: p => p.OwnerId == CurrentUser.Id
                                    || userGroupIds.Contains(p.Group.Id),
                               pageNumber: request.PageNumber,
                               pageSize: request.PageSize,
                               search: request.Search,
                               orderBy: request.OrderBy);

            return ResponseModel<Paginated<DirectoryDto>>.Create(Mapper.Map<Paginated<DirectoryDto>>(directories));
        }
    }
}