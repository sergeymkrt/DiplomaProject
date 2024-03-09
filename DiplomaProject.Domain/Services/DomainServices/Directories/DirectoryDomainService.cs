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
}