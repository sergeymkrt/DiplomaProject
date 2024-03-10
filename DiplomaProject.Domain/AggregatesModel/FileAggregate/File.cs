using DiplomaProject.Domain.AggregatesModel.Keys;
using System.Security.Cryptography;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;

namespace DiplomaProject.Domain.AggregatesModel.FileAggregate;

public class File : Entity, IAggregateRoot
{
    public File()
    {
    }

    public File(string filePath, long directoryId)
    {
        FilePath = filePath;
        DirectoryId = directoryId;
    }

    public File(string filePath, string mimeType, long keyId, long directoryId)
    {
        FilePath = filePath;
        MimeType = mimeType;
        KeyId = keyId;
        DirectoryId = directoryId;
    }

    #region Properties

    public string FilePath { get; set; }
    public string FileDirectory { get; private set; }
    public string MimeType { get; private set; }

    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string FileHash => CalculateHash();

    #endregion

    #region Relationships
    public long DirectoryId { get; set; }
    public virtual Directory Directory { get; set; }

    public long KeyId { get; private set; }
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

    public void SetKey(Key key)
    {
        Key = key;
        KeyId = key.Id;
    }
    #endregion

}