using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.Enums;

namespace DiplomaProject.Domain.Services.DomainServices.Keys;

public interface IKeyDomainService
{
    Task<Key> CreateKey(string name, int keySizeId = (int)KeySize.Size2048);
    Task<Key> UpdateKey(long keyId, string name, int keySizeId);
    Task DeleteKey(long keyId);
    Task<Key> GetKeyById(long keyId);
    Task<Paginated<Key>> GetPaginatedAsync(
        Expression<Func<Key, bool>> predicate = null,
        string search = null,
        List<(string? ColumnName, bool? isAsc)> orderBy = null,
        int pageNumber = 1,
        int pageSize = 10);
}