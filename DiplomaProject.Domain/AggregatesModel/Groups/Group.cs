using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Shared.Lookups;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;

namespace DiplomaProject.Domain.AggregatesModel.Groups;

public class Group : Entity, IAggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }

    public int AccessLevelId { get; set; }
    public AccessLevel AccessLevel { get; set; }

    public string OwnerId { get; set; }
    public virtual User Owner { get; set; }

    public long DirectoryId { get; set; }
    public virtual Directory Directory { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; } = [];
}