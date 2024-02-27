namespace DiplomaProject.Domain.Services.External;

public interface IFileManagementService
{
    Task<string> WriteFileAsync(Stream stream, string fileName);
    Task<Stream> ReadFileAsync(string fileName);
    Task DeleteFileAsync(string fileName);
}