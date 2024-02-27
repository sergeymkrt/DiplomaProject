using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.Encryption;
using System.IO.Compression;
using System.Security.Cryptography;

namespace DiplomaProject.Infrastructure.Shared.Encryption;

public class EncryptionService(ICurrentUser currentUser) : IEncryptionService
{
    public Task<Key> GenerateKeyAsync(int keySize)
    {
        var rsa = RSA.Create(keySize);
        var publicKey = rsa.ExportRSAPublicKey();
        var privateKey = rsa.ExportRSAPrivateKey();
        return Task.FromResult(new Key((KeySize)keySize, Convert.ToBase64String(publicKey), Convert.ToBase64String(privateKey), currentUser.Id));
    }

    public Task<byte[]> EncryptAsync(byte[] data, Key key)
    {
        var aes = Aes.Create();
        var rsa = RSA.Create();

        rsa.ImportRSAPublicKey(Convert.FromBase64String(key.PublicKey), out _);

        var compressedData = Compression(data, CompressionMode.Compress);
        var encryptedData = EncryptAes(compressedData, aes.Key, aes.IV);
        var dataHash = Hash(data);

        var encryptedKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA512);
        var encryptedIv = rsa.Encrypt(aes.IV, RSAEncryptionPadding.OaepSHA512);

        var encrypted = new byte[encryptedKey.Length + encryptedIv.Length + encryptedData.Length + dataHash.Length];
        encryptedKey.CopyTo(encrypted, 0);
        encryptedIv.CopyTo(encrypted, encryptedKey.Length);
        encryptedData.CopyTo(encrypted, encryptedKey.Length + encryptedIv.Length);
        dataHash.CopyTo(encrypted, encryptedKey.Length + encryptedIv.Length + encryptedData.Length);

        return Task.FromResult(encrypted);
    }

    public Task<byte[]> DecryptAsync(byte[] data, Key key)
    {
        // initialize RSA with private key from key
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(key.PrivateKey), out _);

        // Assuming that encryptedKey and encryptedIv each have length of 256
        var encryptedKey = data[..256];
        var encryptedIv = data[256..512];
        var encryptedData = data[512..^32];
        var dataHash = data[^32..];

        // decrypt key and iv
        var keyBytes = rsa.Decrypt(encryptedKey, RSAEncryptionPadding.OaepSHA512);
        var ivBytes = rsa.Decrypt(encryptedIv, RSAEncryptionPadding.OaepSHA512);

        // decrypt data
        var decryptedData = DecryptAes(encryptedData, keyBytes, ivBytes);
        var decompressedData = Compression(decryptedData, CompressionMode.Decompress);

        // check hash
        var hash = Hash(decompressedData);
        if (!hash.SequenceEqual(dataHash))
        {
            throw new Exception("Data has been tampered with");
        }

        return Task.FromResult(decompressedData);
    }

    private static byte[] EncryptAes(byte[] data, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        aes.GenerateKey();

        using var encryptor = aes.CreateEncryptor(key, iv);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(data, 0, data.Length);
        cryptoStream.FlushFinalBlock();
        return memoryStream.ToArray();
    }

    private static byte[] DecryptAes(byte[] data, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(data);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var resultStream = new MemoryStream();
        cryptoStream.CopyTo(resultStream);
        return resultStream.ToArray();
    }

    private static byte[] Compression(byte[] data, CompressionMode mode)
    {
        return mode switch
        {
            CompressionMode.Compress => Compress(data),
            CompressionMode.Decompress => Decompress(data),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    private static byte[] Compress(byte[] data)
    {
        using var outputMemoryStream = new MemoryStream();
        using var compressionStream = new GZipStream(outputMemoryStream, CompressionMode.Compress);
        compressionStream.Write(data, 0, data.Length);
        compressionStream.Close();
        return outputMemoryStream.ToArray();
    }

    private static byte[] Decompress(byte[] data)
    {
        using var inputMemoryStream = new MemoryStream(data);
        using var outputMemoryStream = new MemoryStream();
        using var decompressionStream = new GZipStream(inputMemoryStream, CompressionMode.Decompress);
        decompressionStream.CopyTo(outputMemoryStream);
        return outputMemoryStream.ToArray();
    }


    private static byte[] Hash(byte[] data)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(data);
    }
}