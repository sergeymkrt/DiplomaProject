using DiplomaProject.Domain.Entities.User;
using DiplomaProject.Domain.Shared.Lookups;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Domain.AggregatesModel.Keys;

public class Key : Entity, IAggregateRoot
{
    public Key()
    {
    }

    public Key(int keysize)
    {
        KeySizeID = keysize;
    }

    public Key(Enums.KeySize keysize, string publicKey, string privateKey, string userId)
    {
        KeySizeID = (int)keysize;
        PublicKey = publicKey;
        PrivateKey = privateKey;
        UserId = userId;
    }

    #region Properties

    public string Name { get; set; }
    public string PublicKey { get; private set; }
    public string PrivateKey { get; private set; }
    public string UserId { get; private set; }
    public int KeySizeID { get; set; }

    #endregion

    #region Relationships
    public virtual User User { get; private set; }
    public virtual KeySize KeySize { get; private set; }
    public virtual ICollection<File> Files { get; private set; } = [];
    #endregion

    #region Methods

    public void SetKeys(string publicKey, string privateKey)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }

    public bool IsAccessibleByUser(string userId)
    {
        return UserId == userId;
    }

    #endregion
}