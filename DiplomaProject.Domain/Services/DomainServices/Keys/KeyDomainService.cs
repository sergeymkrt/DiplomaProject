using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Services.Encryption;
using DiplomaProject.Domain.Services.External;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.Domain.Services.DomainServices.Keys;

public class KeyDomainService(
    ICurrentUser currentUser,
    UserManager<User> userManager,
    IKeyRepository keyRepository,
    IEncryptionService encryptionService) : IKeyDomainService
{
    public async Task<Key> CreateKey(string name, int keySizeId = 2048)
    {
        var user = await userManager.Users
            .Include(i => i.Keys)
            .FirstOrDefaultAsync(u => u.Id == currentUser.Id);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var key = await encryptionService.GenerateKeyAsync(keySizeId);
        key.Name = name;
        user.Keys.Add(key);
        await userManager.UpdateAsync(user);
        return key;
    }

    public async Task<Key> UpdateKey(long keyId, string name, int keySizeId)
    {
        var key = await keyRepository.GetByIdAsync(keyId, enableTracking: true);
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (key.UserId != currentUser.Id)
        {
            throw new UnauthorizedAccessException();
        }

        key.Name = name;
        key.KeySizeID = keySizeId;

        return key;
    }

    public Task<Key> GetKeyById(long keyId)
    {
        return keyRepository.GetByIdAsync(keyId);
    }

    public Task<Paginated<Key>> GetPaginatedAsync(
        Expression<Func<Key, bool>> predicate = null,
        string search = null,
        List<(string ColumnName, bool isAsc)> orderBy = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        return keyRepository.GetPaginatedAsync(predicate, search, orderBy, pageNumber, pageSize);
    }

    public async Task DeleteKey(long keyId)
    {
        var key = await keyRepository.GetByIdAsync(keyId);
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (key.UserId != currentUser.Id)
        {
            throw new UnauthorizedAccessException();
        }
        keyRepository.Remove(key);
    }
}