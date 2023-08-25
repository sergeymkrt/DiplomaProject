using DiplomaProject.Infrastructure.Shared.Encryption.Structs;

namespace DiplomaProject.Domain.Services.Encryption;

public interface IEncryptionService
{
    RSAKeyPair GenerateRSA();
}