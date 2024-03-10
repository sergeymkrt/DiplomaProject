using DiplomaProject.Domain.AggregatesModel.Groups;

namespace DiplomaProject.Domain.Shared.Lookups;

public class PermissionType(int id, string name) : Enumeration(id, name)
{
    public virtual ICollection<UserGroup> UserGroups { get; private set; } = [];
}