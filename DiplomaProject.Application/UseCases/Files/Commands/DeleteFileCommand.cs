using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Files;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Files.Commands;

public class DeleteFileCommand(long fileId) : BaseCommand<ResponseModel>
{
    public long FileId { get; set; } = fileId;

    public class DeleteFileCommandHandler(
        IMapper mapper,
        ICurrentUser currentUser,
        IFileDomainService fileDomainService)
        : BaseCommandHandler<DeleteFileCommand>(mapper, currentUser)
    {
        public override async Task<ResponseModel> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            await fileDomainService.DeleteFileAsync(request.FileId);
            return ResponseModel.Create(ResponseCode.SuccessfullyDeleted);
        }
    }
}