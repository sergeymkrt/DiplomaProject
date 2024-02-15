using DiplomaProject.Domain.Helpers;

namespace DiplomaProject.Infrastructure.Persistence.Repositories;

public abstract class BaseQueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : Entity
{
    protected readonly ReadableDbContext Context;
    protected BaseQueryRepository(ReadableDbContext context)
    {
        Context = context;
    }

    public async Task<TEntity> GetByIdAsync(
        long id,
        Expression<Func<TEntity, object>>? include = null)
    {
        var query = BuildQuery(include: include);
        return await query.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TEntity>> GetWhereAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? include = null)
    {
        var query = BuildQuery(predicate: predicate, include: include);
        return await query.ToListAsync();
    }

    public async Task<Paginated<TEntity>> GetPaginatedAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? include = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = BuildQuery(predicate: predicate, include: include);
        return await query.ToPaginateAsync(pageNumber, pageSize);
    }

    private IQueryable<TEntity>? BuildQuery(
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? include = null)
    {
        var set = Context.Set<TEntity>();
        IQueryable<TEntity> query = null;

        if (include != null)
        {
            query = set.Include(include).AsQueryable();
        }

        query ??= set.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query;
    }
}