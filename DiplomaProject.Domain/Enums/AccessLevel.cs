using DiplomaProject.Packages.Attributes;

namespace DiplomaProject.Domain.Enums;

public enum AccessLevel
{
    [EnumDisplay(Name = "Basic")]
    Basic = 0,
    [EnumDisplay(Name = "Confidential")]
    Confidential = 1,
    [EnumDisplay(Name = "Secret")]
    Secret = 2,
    [EnumDisplay(Name = "Top Secret")]
    TopSecret = 3
}