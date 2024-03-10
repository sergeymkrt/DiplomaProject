using DiplomaProject.Domain.AggregatesModel.Groups;

namespace DiplomaProject.Infrastructure.Persistence.Repositories;

public class GroupRepository(AppContext context) : RepositoryBase<Group>(context), IGroupRepository
{
    public override IQueryable<Group> BuildQuery() => Context.Groups
        .Include(g => g.Owner)
        .Include(g => g.UserGroups)
        .ThenInclude(ug => ug.User);
}