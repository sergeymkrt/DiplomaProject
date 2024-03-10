using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;

namespace DiplomaProject.Domain.Services.DomainServices.Directories;

public interface IDirectoryDomainService
{
    Task<Directory> CreateSharedDirectoryAsync(string name, string ownerId, long? parentId);
    Task<Directory> CreatePrivateDirectoryAsync(string name, string userId);

    Task<Directory> GetDirectoryByNameAsync(string name, string userId);
    Task<Directory> GetDirectoryByIdAsync(long directoryId);

    Task<Paginated<Directory>> GetDirectoriesAsync(Expression<Func<Directory, bool>> predicate = null,
        string search = null,
        List<(string? ColumnName, bool? isAsc)> orderBy = null,
        int pageNumber = 1,
        int pageSize = 10);

    Task DeleteDirectoryAsync(long directoryId, string userId);
}