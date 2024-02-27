using DiplomaProject.Application.DTOs.Keys;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.SeedWork;
using DiplomaProject.Domain.Services.DomainServices.Keys;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Keys.Queries;

public class GetKeysQuery(
    int pageNumber,
    int pageSize,
    string search,
    string orderByColumn,
    bool isAsc
    ) : BasePaginatedQuery<KeyDto>(pageNumber, pageSize, search, orderByColumn, isAsc)
{
    public class GetKeysQueryHandler(
        ICurrentUser currentUser,
        IMapper mapper,
        IKeyDomainService keyDomainService)
        : BaseQueryHandler<GetKeysQuery>(currentUser, mapper)
    {
        public override async Task<ResponseModel<Paginated<KeyDto>>> Handle(GetKeysQuery request, CancellationToken cancellationToken)
        {
            var keys = await keyDomainService.GetPaginatedAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                search: request.Search,
                orderBy: request.OrderBy);

            return ResponseModel<Paginated<KeyDto>>.Create(Mapper.Map<Paginated<KeyDto>>(keys));
        }
    }
}
