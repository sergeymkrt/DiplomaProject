using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.FileManagement;
using DiplomaProject.Domain.Services.DomainServices.Files;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Files.Queries;

public class DownloadFileQuery(long fileId) : BaseQuery<FileResponse>
{
    public long FileId { get; set; } = fileId;

    public class DownloadFileQueryHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IFileDomainService fileDomainService)
        : BaseQueryHandler<DownloadFileQuery>(mapper, currentUser)
    {
        public override async Task<ResponseModel<FileResponse>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            var file = await fileDomainService.DownloadFileAsync(request.FileId);
            return ResponseModel<FileResponse>.Create(ResponseCode.Success, FileResponse.CreateFrom(file.Name, file.MimeType, file.ByteArray));
        }
    }
}