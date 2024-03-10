using DiplomaProject.Packages.Attributes;

namespace DiplomaProject.Domain.Enums;

public enum PermissionType
{
    [EnumDisplay(Name = "Editor")]
    Editor = 1,
    [EnumDisplay(Name = "Viewer")]
    Viewer = 2
}