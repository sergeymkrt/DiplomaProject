using DiplomaProject.Domain.Extensions;
using DiplomaProject.Domain.FileManagement;
using DiplomaProject.Domain.Services.Encryption;
using DiplomaProject.Domain.Services.External;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Domain.Services.DomainServices.Files;

public class FileDomainService(
    IFileRepository fileRepository,
    IEncryptionService encryptionService,
    IFileManagementService fileManagementService) : IFileDomainService
{
    public async Task<File> CreateFileAsync(string fileName, string mimeType, long keyId, long directoryId)
    {
        var file = new File(fileName, mimeType, keyId, directoryId);
        await fileRepository.AddAsync(file);
        return file;
    }

    public async Task<string> UploadFileAsync(Stream stream, File file)
    {
        if (file.Key == null)
        {
            throw new ArgumentNullException(nameof(file.Key));
        }

        var streamAsBytes = await stream.ToByteArrayAsync();
        var encryptedData = await encryptionService.EncryptAsync(streamAsBytes, file.Key);
        var encryptedStream = new MemoryStream(encryptedData);
        return await fileManagementService.WriteFileAsync(encryptedStream, file.FileName);
    }

    public async Task DeleteFileAsync(long id)
    {
        var file = await fileRepository.GetByIdAsync(id);

        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        await fileManagementService.DeleteFileAsync(file.FileName);
        fileRepository.Remove(file);
    }

    public async Task<FileResponse> DownloadFileAsync(long fileId)
    {
        var file = await fileRepository.GetByIdAsync(fileId);

        if (file.Key == null)
        {
            throw new ArgumentNullException(nameof(file.Key));
        }

        var encryptedStream = await fileManagementService.ReadFileAsync(file.FileName);
        var encryptedData = await encryptedStream.ToByteArrayAsync();
        var decryptedData = await encryptionService.DecryptAsync(encryptedData, file.Key);

        return FileResponse.CreateFrom(file.FileName, file.MimeType, decryptedData);
    }

    public Task<Paginated<File>> GetFilesAsync(Expression<Func<File, bool>> predicate = null,
        string search = null,
        List<(string? ColumnName, bool? isAsc)> orderBy = null, int pageNumber = 1,
        int pageSize = 10)
    {
        return fileRepository.GetPaginatedAsync(
            predicate: predicate,
            search: search,
            orderBy: orderBy,
            pageNumber: pageNumber,
            pageSize: pageSize);
    }
}