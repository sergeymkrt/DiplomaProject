using DiplomaProject.Application.DTOs.Files;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.SeedWork;
using DiplomaProject.Domain.Services.DomainServices.Files;
using DiplomaProject.Domain.Services.External;
using System.Linq.Expressions;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Application.UseCases.Files.Queries;

public class GetFilesQuery(int pageNumber,
    int pageSize,
    string search,
    string orderByColumn,
    bool isAsc,
    long? directoryId) : BasePaginatedQuery<FileDto>(pageNumber, pageSize, search, orderByColumn, isAsc)
{
    public long? DirectoryId { get; set; } = directoryId;
    public class GetFilesQueryHandler(
        ICurrentUser currentUser,
        IMapper mapper,
        IFileDomainService fileDomainService)
        : BaseQueryHandler<GetFilesQuery>(currentUser, mapper)
    {
        public override async Task<ResponseModel<Paginated<FileDto>>> Handle(GetFilesQuery request,
            CancellationToken cancellationToken)
        {
            var predicate = request.DirectoryId.HasValue
                ? (Expression<Func<File, bool>>)(f => f.DirectoryId == request.DirectoryId)
                : null;

            var files = await fileDomainService.GetFilesAsync(
                               predicate: predicate,
                               pageNumber: request.PageNumber,
                               pageSize: request.PageSize,
                               search: request.Search,
                               orderBy: request.OrderBy);

            return ResponseModel<Paginated<FileDto>>.Create(Mapper.Map<Paginated<FileDto>>(files));
        }
    }
}