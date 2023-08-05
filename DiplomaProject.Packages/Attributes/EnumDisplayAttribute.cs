namespace DiplomaProject.Packages.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class EnumDisplayAttribute : Attribute
{
    public EnumDisplayAttribute()
    {
    }

    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
}