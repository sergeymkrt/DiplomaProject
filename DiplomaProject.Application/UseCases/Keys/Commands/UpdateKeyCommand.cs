using DiplomaProject.Application.DTOs.Keys;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Keys;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Keys.Commands;

public class UpdateKeyCommand(KeyDto keyDto) : BaseCommand<KeyDto>
{
    public KeyDto Dto { get; set; } = keyDto;

    public class UpdateKeyCommandHandler(IMapper mapper, ICurrentUser currentUser,
        IKeyDomainService keyDomainService)
        : BaseCommandHandler<UpdateKeyCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel<KeyDto>> Handle(UpdateKeyCommand request, CancellationToken cancellationToken)
        {
            var key = await keyDomainService.UpdateKey(request.Dto.Id, request.Dto.Name, request.Dto.KeySizeId);
            return ResponseModel<KeyDto>.Create(ResponseCode.SuccessfullyUpdated, Mapper.Map<KeyDto>(key));
        }
    }
}