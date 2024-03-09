using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Directories;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Directories.Commands;

public class CreateDirectoryCommand(string name) : BaseCommand<long>
{
    public string Name { get; set; } = name;
    public class CreateDirectoryCommandHandler(ICurrentUser currentUser, IMapper mapper,
        IDirectoryDomainService directoryDomainService)
        : BaseCommandHandler<CreateDirectoryCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel<long>> Handle(CreateDirectoryCommand request, CancellationToken cancellationToken)
        {
            var existingDirectory = await directoryDomainService.GetDirectoryByNameAsync(request.Name, UserId);
            if (existingDirectory != null)
            {
                return ResponseModel<long>.Create(ResponseCode.Exists, 0, "Directory");
            }

            var directory = await directoryDomainService.CreatePrivateDirectoryAsync(request.Name, UserId);

            return ResponseModel<long>.Create(ResponseCode.SuccessfullyCreated, directory.Id);
        }
    }
}