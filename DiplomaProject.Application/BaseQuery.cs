using DiplomaProject.Application.Models;
using DiplomaProject.Domain.SeedWork;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application;

public abstract class BaseQuery<TResponse> : IRequest<ResponseModel<TResponse>>
{
    public abstract class BaseQueryHandler<TRequest>(
        IMapper mapper,
        ICurrentUser currentUser) : IRequestHandler<TRequest, ResponseModel<TResponse>>
        where TRequest : BaseQuery<TResponse>
    {
        protected readonly IMapper Mapper = mapper;
        protected readonly ICurrentUser CurrentUser = currentUser;

        public abstract Task<ResponseModel<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

public abstract class BasePaginatedQuery<TResponse>(
    int pageNumber = 1,
    int pageSize = 10,
    string search = null,
    string orderByColumn = null,
    bool isAsc = false)
    : IRequest<ResponseModel<Paginated<TResponse>>>
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public string Search { get; } = search;
    public string OrderByColumn { get; } = orderByColumn;
    public bool IsAsc { get; } = isAsc;
    public List<(string ColumnName, bool isAsc)> OrderBy { get; } = [(orderByColumn, isAsc)];

    public abstract class BaseQueryHandler<TRequest>(ICurrentUser currentUser, IMapper mapper)
        : IRequestHandler<TRequest, ResponseModel<Paginated<TResponse>>>
        where TRequest : BasePaginatedQuery<TResponse>
    {
        protected readonly IMapper Mapper = mapper;
        protected readonly ICurrentUser CurrentUser = currentUser;

        public abstract Task<ResponseModel<Paginated<TResponse>>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}