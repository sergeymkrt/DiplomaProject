using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application;

public abstract class BaseCommand : IBaseCommand, IRequest<ResponseModel>
{
    public abstract class BaseCommandHandler<TRequest>(ICurrentUser currentUser, IMapper mapper)
        : IRequestHandler<TRequest, ResponseModel>
        where TRequest : BaseCommand
    {
        protected readonly IMapper Mapper = mapper;
        protected readonly ICurrentUser CurrentUser = currentUser;
        protected string? UserId => CurrentUser.Id;

        public abstract Task<ResponseModel> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

public abstract class BaseCommand<TResponse> : IBaseCommand, IRequest<ResponseModel<TResponse>>
{
    public abstract class BaseCommandHandler<TRequest>(ICurrentUser currentUser, IMapper mapper)
        : IRequestHandler<TRequest, ResponseModel<TResponse>>
        where TRequest : BaseCommand<TResponse>
    {
        protected readonly IMapper Mapper = mapper;
        protected readonly ICurrentUser CurrentUser = currentUser;
        protected string? UserId => CurrentUser.Id;

        public abstract Task<ResponseModel<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);

    }
}

public interface IBaseCommand
{
}