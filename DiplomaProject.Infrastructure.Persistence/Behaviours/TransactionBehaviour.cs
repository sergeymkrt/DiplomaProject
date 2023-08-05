using DiplomaProject.Application;
using DiplomaProject.Infrastructure.Persistence.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.Infrastructure.Persistence.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly WritableDbContext _dbContext;

    public TransactionBehaviour(WritableDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentException(nameof(WritableDbContext));
    }

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

                    await _dbContext.CommitTransactionAsync(transaction);

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