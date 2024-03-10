using DiplomaProject.Domain.FileManagement;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Domain.Services.DomainServices.Files;

public interface IFileDomainService
{
    Task<File> CreateFileAsync(string fileName, string mimeType, long keyId, long directoryId);
    Task<string> UploadFileAsync(Stream stream, File file);
    Task DeleteFileAsync(long fileId);
    Task<FileResponse> DownloadFileAsync(long fileId);

    Task<Paginated<File>> GetFilesAsync(Expression<Func<File, bool>> predicate = null,
        string search = null,
        List<(string ColumnName, bool isAsc)> orderBy = null,
        int pageNumber = 1,
        int pageSize = 10);
}