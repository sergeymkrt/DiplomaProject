namespace DiplomaProject.Domain.Attributes;


[AttributeUsage(AttributeTargets.Field)]
public class AcceptedNamesAttribute : Attribute
{
    public AcceptedNamesAttribute(params string[] names)
    {
        foreach (var name in names)
        {
            Names.Add(name);
        }
    }

    public List<string> Names { get; set; } = [];
}

