namespace DiplomaProject.Domain.Services.External;

public interface IFileManagementService
{
    Task WriteFileAsync(string path, Stream stream);
    Task<Stream> ReadFileAsync(string path);
}