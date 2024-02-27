using DiplomaProject.Domain.AggregatesModel.Keys;

namespace DiplomaProject.Domain.Services.Encryption;

public interface IEncryptionService
{
    Task<Key> GenerateKeyAsync(int keySize);
    Task<byte[]> EncryptAsync(byte[] data, Key key);
    Task<byte[]> DecryptAsync(byte[] data, Key key);
}