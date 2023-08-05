using DiplomaProject.Domain.Helpers;

namespace DiplomaProject.Infrastructure.Persistence.Extensions;

public static class PaginateExtensions
{
    public static async Task<Paginated<T>> ToPaginateAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalRecords = await source.CountAsync(cancellationToken);

        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new Paginated<T>(items,totalRecords, pageNumber, pageSize);
    }
}