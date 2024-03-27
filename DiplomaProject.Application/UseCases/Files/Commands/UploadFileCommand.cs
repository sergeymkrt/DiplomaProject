using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Files;
using DiplomaProject.Domain.Services.DomainServices.Keys;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Http;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Application.UseCases.Files.Commands;

public class UploadFileCommand(IFormFile file, long? directoryId) : BaseCommand<File>
{
    public IFormFile File { get; set; } = file;
    // public long KeyId { get; set; } = keyId;
    public long? DirectoryId { get; set; } = directoryId;

    public class UploadFileCommandHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IKeyDomainService keyDomainService,
        IFileDomainService fileDomainService)
        : BaseCommandHandler<UploadFileCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel<File>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var key = await keyDomainService.CreateKey(request.File.FileName);
            var user = await CurrentUser.GetUserWithRelations();
            if (!request.DirectoryId.HasValue)
            {
                request.DirectoryId = user.PersonalDirectoryId;
            }


            var fileObject = await fileDomainService.CreateFileAsync(
                request.File.FileName,
                request.File.ContentType,
                // request.KeyId,
                key,
                request.DirectoryId.GetValueOrDefault());

            // var key = await keyDomainService.GetKeyById(request.KeyId);
            fileObject.SetKey(key);
            fileObject.FileName = request.File.FileName;
            fileObject.FileSize = request.File.Length;

            var ms = new MemoryStream();
            await using (var fs = request.File.OpenReadStream())
            {
                await fs.CopyToAsync(ms, cancellationToken);
            }

            var path = await fileDomainService.UploadFileAsync(ms, fileObject);
            fileObject.FilePath = path;

            return ResponseModel<File>.Create(ResponseCode.SuccessfullyUploaded, Mapper.Map<File>(fileObject));
        }
    }

}