using System.Runtime.Serialization;

namespace DiplomaProject.Packages.Exceptions;

[Serializable]
public class EnumParseException : Exception
{
    protected EnumParseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
    public EnumParseException(string message) : base(message)
    {

    }
}