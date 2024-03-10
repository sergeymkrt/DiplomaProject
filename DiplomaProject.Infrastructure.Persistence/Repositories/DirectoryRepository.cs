using DiplomaProject.Domain.AggregatesModel.Directories;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;

namespace DiplomaProject.Infrastructure.Persistence.Repositories;

public class DirectoryRepository(AppContext context) : RepositoryBase<Directory>(context), IDirectoryRepository
{
    public override IQueryable<Directory> BuildQuery() => Context.Directories
        .Include(d => d.Owner)
        .Include(d => d.PersonalOwner)
        .Include(d => d.Group)
        .Include(d => d.Directories)
        .Include(d => d.Files);
}