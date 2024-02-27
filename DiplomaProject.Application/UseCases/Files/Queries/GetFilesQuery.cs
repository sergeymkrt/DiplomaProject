using DiplomaProject.Application.DTOs.Files;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.SeedWork;
using DiplomaProject.Domain.Services.DomainServices.Files;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Files.Queries;

public class GetFilesQuery(int pageNumber,
    int pageSize,
    string search,
    string orderByColumn,
    bool isAsc) : BasePaginatedQuery<FileDto>(pageNumber, pageSize, search, orderByColumn, isAsc)
{
    public class GetFilesQueryHandler(
        ICurrentUser currentUser,
        IMapper mapper,
        IFileDomainService fileDomainService)
        : BaseQueryHandler<GetFilesQuery>(currentUser, mapper)
    {
        public override async Task<ResponseModel<Paginated<FileDto>>> Handle(GetFilesQuery request,
            CancellationToken cancellationToken)
        {
            var files = await fileDomainService.GetFilesAsync(
                               pageNumber: request.PageNumber,
                                              pageSize: request.PageSize,
                                              search: request.Search,
                                              orderBy: request.OrderBy);

            return ResponseModel<Paginated<FileDto>>.Create(Mapper.Map<Paginated<FileDto>>(files));
        }
    }
}