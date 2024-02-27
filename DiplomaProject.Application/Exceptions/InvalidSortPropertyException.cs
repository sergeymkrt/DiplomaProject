using DiplomaProject.Domain.Enums;
using System.Runtime.Serialization;

namespace DiplomaProject.Application.Exceptions;

[Serializable]
public class InvalidSortPropertyException(string propertyName, SortExceptionCode code) : Exception(GetMessage(propertyName, code)), ISerializable
{
    public string PropertyName { get; set; } = propertyName;
    public SortExceptionCode Code { get; set; } = code;

    public static string GetMessage(string propertyName, SortExceptionCode code)
    {
        return code switch
        {
            SortExceptionCode.InvalidSortingProperty => $"Invalid sorting property {propertyName}",
            SortExceptionCode.NonSortingProperty => $"{propertyName} is not a sorting property: ",
            _ => throw new Exception("Not implemented case"),
        };
    }
}
