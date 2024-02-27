namespace DiplomaProject.Domain.FileManagement;

public class FileResponse
{
    public string Name { get; }
    public string MimeType { get; }
    public byte[] ByteArray { get; }

    private FileResponse(
        string name,
        string mimeType,
        byte[] byteArray)
    {
        Name = name;
        MimeType = mimeType;
        ByteArray = byteArray;
    }

    public static FileResponse CreateFrom(
        string name,
        string mimeType,
        byte[] byteArray)
    {
        return new FileResponse(name, mimeType, byteArray);
    }
}
