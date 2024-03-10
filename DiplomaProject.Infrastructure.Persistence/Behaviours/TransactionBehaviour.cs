namespace DiplomaProject.Infrastructure.Persistence.Behaviours;

public class TransactionBehaviour<TRequest, TResponse>(AppContext dbContext, ICurrentUser currentUser) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly AppContext _dbContext = dbContext ?? throw new ArgumentException(nameof(AppContext));

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var type = typeof(TRequest);
        if (type.BaseType.GetGenericTypeDefinition() == typeof(BaseQuery<>))
        {
            return await next();
        }

        var response = default(TResponse);

        try
        {
            if (_dbContext.HasActiveTransaction)
            {
                return await next();
            }

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using (var transaction = await _dbContext.BeginTransactionAsync())
                {
                    response = await next();

                    await _dbContext.CommitTransactionAsync(transaction, currentUser.Id);

                    transactionId = transaction.TransactionId;
                }
            });

            return response;
        }
        catch
        {
            throw;
        }
    }
}