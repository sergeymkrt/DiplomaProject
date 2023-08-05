using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application;

public abstract class BaseCommand<TResponse> : IRequest<TResponse>// where TResponse : IResponseDto
{
    public abstract class BaseCommandHandler<TRequest> : IRequestHandler<TRequest, TResponse>
        where TRequest : BaseCommand<TResponse>
    {
        protected readonly IMapper Mapper;
        protected readonly IIdentityUserService IdentityUser;
        protected BaseCommandHandler(
            IMapper mapper,
            IIdentityUserService identityUser)
        {
            Mapper = mapper;
            IdentityUser = identityUser;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

    }
}