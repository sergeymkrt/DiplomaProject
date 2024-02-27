using DiplomaProject.Domain.AggregatesModel.FileAggregate;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Infrastructure.Persistence.Repositories;

public class FileRepository(AppContext context) : RepositoryBase<File>(context), IFileRepository
{
    public override IQueryable<File> BuildQuery()
        => Context.Files
            .Include(f => f.Key);
}