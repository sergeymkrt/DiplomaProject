using DiplomaProject.Packages.Attributes;

namespace DiplomaProject.Domain.Enums;

public enum AccessLevel
{
    [EnumDisplay(Name = "Basic")]
    Basic = 1,
    [EnumDisplay(Name = "Confidential")]
    Confidential = 2,
    [EnumDisplay(Name = "Secret")]
    Secret = 3,
    [EnumDisplay(Name = "Top Secret")]
    TopSecret = 4
}