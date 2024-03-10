using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Shared.Lookups;

namespace DiplomaProject.Domain.AggregatesModel.Groups;

public class UserGroup
{
    public UserGroup()
    {
    }

    public UserGroup(string userId, int permissionType)
    {
        UserId = userId;
        PermissionTypeId = permissionType;
    }

    public string UserId { get; set; }
    public virtual User User { get; set; }

    public long GroupId { get; set; }
    public virtual Group Group { get; set; }

    public int PermissionTypeId { get; set; }
    public virtual PermissionType PermissionType { get; set; }
}