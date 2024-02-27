namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class FileManagementService : IFileManagementService
{
    private const string BasePath = "Files";

    public Task<string> WriteFileAsync(Stream stream, string fileName)
    {
        var filePath = Path.Combine(BasePath, fileName);
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);
        return Task.FromResult(filePath);
    }

    public Task<Stream> ReadFileAsync(string fileName)
    {
        var filePath = Path.Combine(BasePath, fileName);
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return Task.FromResult<Stream>(fileStream);
    }

    public Task DeleteFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}