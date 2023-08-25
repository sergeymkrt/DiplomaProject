using DiplomaProject.Domain.Services.Encryption;
using DiplomaProject.Infrastructure.Shared.Encryption.Structs;

namespace DiplomaProject.Infrastructure.Shared.Encryption;

public class EncryptionService : IEncryptionService
{
    public RSAKeyPair GenerateRSA()
    {
        return Encryption.generateRSA();
    }
}