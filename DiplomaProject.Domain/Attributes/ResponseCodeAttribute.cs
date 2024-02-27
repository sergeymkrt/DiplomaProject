using DiplomaProject.Domain.Enums;

namespace DiplomaProject.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ResponseCodeAttribute(ResponseType responseType, bool isFormattable = false) : Attribute
{
    public ResponseType ResponseType { get; private set; } = responseType;
    public bool IsFormattable { get; private set; } = isFormattable;
}
