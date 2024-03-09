using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;

namespace DiplomaProject.Domain.Services.DomainServices.Directories;

public interface IDirectoryDomainService
{
    Task<Directory> CreateSharedDirectoryAsync(string name, string ownerId, long? parentId);
    Task<Directory> CreatePrivateDirectoryAsync(string name, string userId);

    Task<Directory> GetDirectoryByNameAsync(string name, string userId);
}