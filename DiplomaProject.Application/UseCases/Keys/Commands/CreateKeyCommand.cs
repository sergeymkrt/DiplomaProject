using DiplomaProject.Application.DTOs.Keys;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Keys;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Keys.Commands;

public class CreateKeyCommand(KeyDto dto) : BaseCommand<KeyDto>
{
    public KeyDto Dto { get; set; } = dto;

    public class CreateKeyCommandHandler(IMapper mapper,
        ICurrentUser currentUser,
        IKeyDomainService keyDomainService)
        : BaseCommandHandler<CreateKeyCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel<KeyDto>> Handle(CreateKeyCommand request, CancellationToken cancellationToken)
        {
            var key = await keyDomainService.CreateKey(request.Dto.Name, request.Dto.KeySizeId);
            return ResponseModel<KeyDto>.Create(ResponseCode.SuccessfullyCreated, Mapper.Map<KeyDto>(key));
        }
    }
}