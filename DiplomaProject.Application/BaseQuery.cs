using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application;

public abstract class BaseQuery<TResponse> : IRequest<TResponse>
{
    public abstract class BaseQueryHandler<TRequest>(
        IMapper mapper,
        ICurrentUser currentUser) : IRequestHandler<TRequest, TResponse>
        where TRequest : BaseQuery<TResponse>
    {
        protected readonly IMapper Mapper = mapper;
        protected readonly ICurrentUser CurrentUser = currentUser;

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}