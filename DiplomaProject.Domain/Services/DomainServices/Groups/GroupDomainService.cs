using DiplomaProject.Domain.AggregatesModel.Groups;
using DiplomaProject.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace DiplomaProject.Domain.Services.DomainServices.Groups;

public class GroupDomainService(
    UserManager<User> userManager,
    IGroupRepository groupRepository) : IGroupDomainService
{
    public async Task RemoveUserFromGroup(string userId, long groupId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var group = await groupRepository.GetByIdAsync(groupId);

        if (group is null)
        {
            throw new NotFoundException("Group not found");
        }

        group.RemoveUser(user);
    }

    public async Task AddUserToGroup(string userId, long groupId, int permissionId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var group = await groupRepository.GetByIdAsync(groupId);

        if (group is null)
        {
            throw new NotFoundException("Group not found");
        }

        group.AddUser(user, permissionId);
    }

    public Task CreateGroup(string groupName, string description, int accessLevelId, User owner)
    {
        var group = new Group(groupName, description, accessLevelId, owner);
        groupRepository.AddAsync(group);
        return Task.CompletedTask;
    }

    public async Task DeleteGroup(long groupId, string userId)
    {
        var group = await groupRepository.GetByIdAsync(groupId);
        if (group is null)
        {
            throw new NotFoundException("Group not found");
        }

        if (group.OwnerId != userId)
        {
            throw new DomainException("You are not the owner of this group");
        }

        groupRepository.Remove(group);
    }

    public Task<Paginated<Group>> GetGroups(Expression<Func<Group, bool>> predicate = null, string search = null, List<(string ColumnName, bool isAsc)> orderBy = null, int pageNumber = 1,
        int pageSize = 10)
    {
        return groupRepository.GetPaginatedAsync(
                       predicate: predicate,
                       search: search,
                       orderBy: orderBy,
                       pageNumber: pageNumber,
                       pageSize: pageSize);
    }
}