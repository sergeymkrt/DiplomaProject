using System.Linq.Expressions;

namespace DiplomaProject.Domain.SeedWork;

public interface IQueryRepository<T> where T : Entity
{
    Task<T> GetByIdAsync(
        long id,
        Expression<Func<T, object>>? include = null);

    Task<List<T>> GetWhereAsync(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? include = null);
}