using DiplomaProject.Domain.AggregatesModel.Groups;
using DiplomaProject.Domain.Entities.User;

namespace DiplomaProject.Domain.Services.DomainServices.Groups;


public interface IGroupDomainService
{
    Task RemoveUserFromGroup(string userId, long groupId);
    Task AddUserToGroup(string userId, long groupId, int permissionId);
    Task CreateGroup(string groupName, string description, int accessLevelId, User owner);
    Task DeleteGroup(long groupId, string userId);
    Task<Group> GetGroup(long groupId);
    Task<Paginated<Group>> GetGroups(Expression<Func<Group, bool>> predicate = null,
        string search = null,
        List<(string? ColumnName, bool? isAsc)> orderBy = null,
        int pageNumber = 1,
        int pageSize = 10);
}