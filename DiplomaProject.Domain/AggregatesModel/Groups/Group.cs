using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Shared.Lookups;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;

namespace DiplomaProject.Domain.AggregatesModel.Groups;

public class Group : Entity, IAggregateRoot
{
    public Group()
    {
    }

    public Group(string name, string description, int accessLevelId, User owner)
    {
        if (owner.AccessLevelId < accessLevelId)
        {
            throw new DomainException("Owner access level is lower than group access level");
        }

        Name = name;
        Description = description;
        AccessLevelId = accessLevelId;
        OwnerId = owner.Id;

        Directory = new Directory(name, owner.Id, null);
    }

    public string Name { get; set; }
    public string Description { get; set; }

    public int AccessLevelId { get; set; }
    public AccessLevel AccessLevel { get; set; }

    public string OwnerId { get; set; }
    public virtual User Owner { get; set; }

    public long DirectoryId { get; set; }
    public virtual Directory Directory { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; } = [];

    public void RemoveUser(User user)
    {
        var userGroup = UserGroups.FirstOrDefault(x => x.UserId == user.Id);
        if (userGroup != null)
        {
            UserGroups.Remove(userGroup);
        }
    }

    public void AddUser(User user, int permissionId)
    {
        UserGroups.Add(new UserGroup(user.Id, permissionId));
    }
}