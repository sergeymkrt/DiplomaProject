namespace DiplomaProject.Domain.Attributes;


public class MappingValueAttribute(int value) : Attribute
{
    public int Value { get; private set; } = value;
}
