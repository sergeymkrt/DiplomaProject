using DiplomaProject.Domain.AggregatesModel.BlackLists;

namespace DiplomaProject.Infrastructure.Persistence.Repositories;

public class BlackListRepository(AppContext context) : RepositoryBase<BlackList>(context), IBlackListRepository
{
    public override IQueryable<BlackList> BuildQuery() => Context.BlackLists;
}