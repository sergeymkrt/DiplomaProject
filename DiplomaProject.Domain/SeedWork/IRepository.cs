namespace DiplomaProject.Domain.SeedWork;

public interface IRepository<T> where T : IAggregateRoot
{
    Task AddAsync(T item);
    Task<T?> GetByIdAsync(long id);
}