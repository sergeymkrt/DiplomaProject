using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application;

public abstract class BaseCommand<TResponse> : IRequest<TResponse>// where TResponse : IResponseDto
{
    public abstract class BaseCommandHandler<TRequest>(
        IMapper mapper,
        ICurrentUser currentUser) : IRequestHandler<TRequest, TResponse>
        where TRequest : BaseCommand<TResponse>
    {
        protected readonly IMapper Mapper = mapper;
        protected readonly ICurrentUser CurrentUser = currentUser;

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

    }
}