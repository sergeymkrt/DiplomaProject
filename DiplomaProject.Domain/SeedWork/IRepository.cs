using DiplomaProject.Domain.Models;

namespace DiplomaProject.Domain.SeedWork;

public interface IRepository<TEntity>
    where TEntity : Entity
{
    IUnitOfWork UnitOfWork { get; }

    Task<TEntity> GetByIdAsync(
        long id,
        bool enableTracking = false);

    Task<bool> ExistsAsync(long id);

    Task<Paginated<TEntity>> GetPaginatedAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        string search = null,
        List<(string ColumnName, bool isAsc)> orderBy = null,
        int depth = 2,
        int pageNumber = 1,
        int pageSize = 10,
        IEnumerable<SearchExpression> extraSearchExpressions = default);

    IQueryable<TEntity> GetQueryable(
        Expression<Func<TEntity, bool>> predicate = null,
        string search = null,
        List<(string ColumnName, bool isAsc)> orderBy = null,
        int depth = 2,
        IEnumerable<SearchExpression> extraSearchExpressions = default,
        bool enableTracking = false);

    IQueryable<TEntity> GetAll(
        string search,
        Expression<Func<TEntity, bool>> predicate = null,
        int depth = 2,
        IEnumerable<SearchExpression> extraSearchExpressions = default,
        bool enableTracking = false);

    IQueryable<TEntity> GetAll(
        ICollection<string> columns,
        string search,
        IEnumerable<SearchExpression> extraSearchExpressions = default,
        bool enableTracking = false);

    Task<List<TEntity>> GetAllAsync(
        string search,
        Expression<Func<TEntity, bool>> predicate = null,
        IEnumerable<SearchExpression> extraSearchExpressions = default,
        bool enableTracking = false);

    Task<List<TResult>> GetAllAsync<TResult>(
        string search,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        IEnumerable<SearchExpression> extraSearchExpressions = default);

    IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> expression);

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<decimal> SumAsync(
               Expression<Func<TEntity, decimal>> selector,
               Expression<Func<TEntity, bool>> predicate = null,
               CancellationToken cancellationToken = default);
}