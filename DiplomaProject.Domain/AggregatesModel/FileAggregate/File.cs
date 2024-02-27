using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.Entities.User;
using System.Security.Cryptography;

namespace DiplomaProject.Domain.AggregatesModel.FileAggregate;

public class File : Entity, IAggregateRoot
{
    public File()
    {
    }
    public File(string filePath, string userId, string? fileDirectory = null)
    {
        FilePath = filePath;
        UserId = userId;
        FileDirectory = fileDirectory;
    }

    public File(string filePath, string mimeType, long keyId, string userId, string directory = null)
    {
        FilePath = filePath;
        MimeType = mimeType;
        KeyId = keyId;
        UserId = userId;
        FileDirectory = directory;
    }

    #region Properties

    public string FilePath { get; set; }
    public string FileDirectory { get; private set; }
    public string MimeType { get; private set; }
    public string UserId { get; private set; }
    public long KeyId { get; private set; }

    public string FileName { get; set; }
    public long FileSize { get; set; }
    // public long FileSize => new FileInfo(FilePath).Length;
    public string FileHash => CalculateHash();

    #endregion

    #region Relationships
    public virtual User User { get; private set; }
    public virtual Key Key { get; private set; }
    #endregion

    #region Methods

    private string CalculateHash()
    {
        using var hashAlgorithm = SHA256.Create();
        using var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        byte[] hashValue = hashAlgorithm.ComputeHash(fileStream);
        return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
    }

    public bool IsAccessibleByUser(string userId)
    {
        return UserId == userId;
    }

    public void SetKey(Key key)
    {
        Key = key;
        KeyId = key.Id;
    }

    public void SetProps(string mimeType, string userId, string directory)
    {
        MimeType = mimeType;
        UserId = userId;
        FileDirectory = directory;
    }

    #endregion

}