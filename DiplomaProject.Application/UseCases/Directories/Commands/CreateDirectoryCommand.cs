using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Directories;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Directories.Commands;

public class CreateDirectoryCommand(string name, long? parentId) : BaseCommand
{
    public string Name { get; set; } = name;
    public long? ParentId { get; set; } = parentId;
    public class CreateDirectoryCommandHandler(ICurrentUser currentUser, IMapper mapper,
        IDirectoryDomainService directoryDomainService)
        : BaseCommandHandler<CreateDirectoryCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel> Handle(CreateDirectoryCommand request, CancellationToken cancellationToken)
        {
            var existingDirectory = await directoryDomainService.GetDirectoryByNameAsync(request.Name, UserId);
            if (existingDirectory != null)
            {
                return ResponseModel.Create(ResponseCode.Exists, "Directory");
            }

            await directoryDomainService.CreateSharedDirectoryAsync(request.Name, UserId, request.ParentId);

            return ResponseModel.Create(ResponseCode.SuccessfullyCreated);
        }
    }
}