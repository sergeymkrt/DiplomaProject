using DiplomaProject.Packages.Attributes;

namespace DiplomaProject.Domain.Enums;

public enum KeySize
{
    [EnumDisplay(Name = "RSA 2048")]
    Size2048 = 2048,
    [EnumDisplay(Name = "RSA 3072")]
    Size3072 = 3072,
    [EnumDisplay(Name = "RSA 7680")]
    Size7680 = 7680,
    [EnumDisplay(Name = "RSA 15360")]
    Size15360 = 15360
}