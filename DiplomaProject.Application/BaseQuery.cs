using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application;

public abstract class BaseQuery<TResponse> : IRequest<TResponse>
{
    public abstract class BaseQueryHandler<TRequest> : IRequestHandler<TRequest, TResponse>
        where TRequest : BaseQuery<TResponse>
    {
        protected readonly IMapper Mapper;
        protected readonly IIdentityUserService IdentityUserService;

        protected BaseQueryHandler(
            IMapper mapper,
            IIdentityUserService identityService)
        {
            Mapper = mapper;
            IdentityUserService = identityService;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}