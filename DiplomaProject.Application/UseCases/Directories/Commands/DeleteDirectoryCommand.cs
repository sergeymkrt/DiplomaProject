using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Directories;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Directories.Commands;

public class DeleteDirectoryCommand(long id) : BaseCommand
{
    public long DirectoryId { get; set; } = id;

    public class DeleteDirectoryCommandHandler(
        ICurrentUser currentUser,
        IMapper mapper,
        IDirectoryDomainService directoryDomainService)
        : BaseCommandHandler<DeleteDirectoryCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel> Handle(DeleteDirectoryCommand request,
            CancellationToken cancellationToken)
        {
            await directoryDomainService.DeleteDirectoryAsync(request.DirectoryId, UserId);
            return ResponseModel.Create(ResponseCode.SuccessfullyDeleted);
        }
    }
}