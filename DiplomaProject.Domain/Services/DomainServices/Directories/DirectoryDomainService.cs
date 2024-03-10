using DiplomaProject.Domain.AggregatesModel.Directories;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;

namespace DiplomaProject.Domain.Services.DomainServices.Directories;

public class DirectoryDomainService(IDirectoryRepository directoryRepository) : IDirectoryDomainService
{
    public Task<Directory> CreateSharedDirectoryAsync(string name, string ownerId, long? parentId)
    {
        var directory = new Directory(name, ownerId, parentId);
        return Task.FromResult(directory);
    }

    public Task<Directory> CreatePrivateDirectoryAsync(string name, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Directory> GetDirectoryByNameAsync(string name, string userId)
    {
        return Task.FromResult(directoryRepository.GetWhere(d => d.Name == name && d.OwnerId == userId).FirstOrDefault());
    }

    public Task<Directory> GetDirectoryByIdAsync(long directoryId)
    {
        return directoryRepository.GetByIdAsync(directoryId);
    }

    public Task<Paginated<Directory>> GetDirectoriesAsync(Expression<Func<Directory, bool>> predicate = null, string search = null, List<(string ColumnName, bool isAsc)> orderBy = null, int pageNumber = 1,
        int pageSize = 10)
    {
        return directoryRepository.GetPaginatedAsync(
            predicate: predicate,
            search: search,
            orderBy: orderBy,
            pageNumber: pageNumber,
            pageSize: pageSize);
    }

    public async Task DeleteDirectoryAsync(long directoryId, string currentUserId)
    {
        var directory = await directoryRepository.GetByIdAsync(directoryId);

        if (directory == null)
        {
            throw new NotFoundException("Directory");
        }

        if (directory.Directories.Count != 0)
        {
            throw new DomainException("Directory not empty.");
        }

        if (directory.OwnerId != currentUserId)
        {
            throw new DomainException("You are not the owner.");
        }

        directoryRepository.Remove(directory);
    }
}