namespace DiplomaProject.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SearchColumnAttribute : Attribute
{
    public SearchColumnAttribute()
    {
    }
}

