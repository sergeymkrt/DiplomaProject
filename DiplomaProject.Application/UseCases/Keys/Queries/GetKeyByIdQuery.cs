using DiplomaProject.Application.DTOs.Keys;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.DomainServices.Keys;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Keys.Queries;

public class GetKeyByIdQuery(long keyId) : BaseQuery<KeyDto>
{
    public long KeyId { get; set; } = keyId;

    public class GetKeyByIdQueryHandler(IMapper mapper, ICurrentUser currentUser, IKeyDomainService keyDomainService)
        : BaseQueryHandler<GetKeyByIdQuery>(mapper, currentUser)
    {
        public override async Task<ResponseModel<KeyDto>> Handle(GetKeyByIdQuery request, CancellationToken cancellationToken)
        {
            var key = await keyDomainService.GetKeyById(request.KeyId);

            return ResponseModel<KeyDto>.Create(ResponseCode.Success, Mapper.Map<KeyDto>(key));
        }
    }
}