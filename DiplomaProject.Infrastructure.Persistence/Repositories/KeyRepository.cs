using DiplomaProject.Domain.AggregatesModel.Keys;

namespace DiplomaProject.Infrastructure.Persistence.Repositories;

public class KeyRepository(AppContext context) : RepositoryBase<Key>(context), IKeyRepository
{
    public override IQueryable<Key> BuildQuery()
        => Context.Keys.Include(k => k.KeySize);
}