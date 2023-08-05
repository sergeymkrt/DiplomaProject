namespace DiplomaProject.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity>: IRepository<TEntity> where TEntity : class, IAggregateRoot
{
    protected readonly AppContext Context;

    public BaseRepository(AppContext context)
    {
        Context = context;
    }
    public async Task AddAsync(TEntity item)
    {
        await Context.Set<TEntity>().AddAsync(item);
        await Context.SaveChangesAsync();
    }

    public async Task<TEntity?> GetByIdAsync(long id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }
}