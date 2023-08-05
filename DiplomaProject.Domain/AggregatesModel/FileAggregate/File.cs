using System.Security.Cryptography;
using DiplomaProject.Domain.Entities.User;

namespace DiplomaProject.Domain.AggregatesModel.FileAggregate;

public class File : Entity, IAggregateRoot
{
    public File()
    {
    }
    public File(string filePath, string userId)
    {
        FilePath = filePath;
        UserId = userId;
    }

    #region Properties

    public string FilePath { get; private set; }
    public string UserId { get; private set; }
    public string FileName => Path.GetFileName(FilePath);
    public long FileSize => new FileInfo(FilePath).Length;
    public string FileHash => CalculateHash();

    #endregion

    #region Relationships
    public virtual User User { get; private set; }
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

    public void Delete()
    {
        // Delete the file from the server securely
        System.IO.File.Delete(FilePath);
    }    

    #endregion
    
}