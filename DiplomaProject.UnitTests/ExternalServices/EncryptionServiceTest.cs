using Autofac.Extras.Moq;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Infrastructure.Shared.Encryption;
using System.Text;
using Xunit.Abstractions;

namespace DiplomaProject.UnitTests.ExternalServices;

public class EncryptionServiceTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task GenerateKeyPair_ShouldReturnKeyPair()
    {
        using var mock = AutoMock.GetLoose();

        // Arrange
        var keySize = KeySize.Size2048;

        var encryptionService = mock.Create<EncryptionService>();

        // Act
        var key = await encryptionService.GenerateKeyAsync((int)keySize);

        // Assert
        Assert.Equal((int)keySize, key.KeySizeID);
        Assert.NotNull(key);
        Assert.NotNull(key.PublicKey);
        Assert.NotNull(key.PrivateKey);
    }

    [Fact]
    public async Task EncryptAsync_ShouldReturnEncryptedData()
    {
        using var mock = AutoMock.GetLoose();

        // Arrange
        const KeySize keySize = KeySize.Size2048;
        var data = "Hello, World!"u8.ToArray();

        var encryptionService = mock.Create<EncryptionService>();

        var key = await encryptionService.GenerateKeyAsync((int)keySize);

        // Act
        var encryptedData = await encryptionService.EncryptAsync(data, key);

        // Assert
        Assert.NotNull(encryptedData);
        Assert.NotEmpty(encryptedData);
    }

    [Fact]
    public async Task DecryptAsync_ShouldReturnDecryptedData()
    {
        using var mock = AutoMock.GetLoose();

        // Arrange
        const KeySize keySize = KeySize.Size2048;
        var data = "Hello, World!"u8.ToArray();

        var encryptionService = mock.Create<EncryptionService>();

        var key = await encryptionService.GenerateKeyAsync((int)keySize);
        var encryptedData = await encryptionService.EncryptAsync(data, key);

        // Act
        var decryptedData = await encryptionService.DecryptAsync(encryptedData, key);

        // Assert
        Assert.NotNull(decryptedData);
        Assert.NotEmpty(decryptedData);
        Assert.Equal(data, decryptedData);
    }

    [Fact]
    public async Task EncryptAsync_ShouldReturnDifferentEncryptedData()
    {
        using var mock = AutoMock.GetLoose();

        // Arrange
        const KeySize keySize = KeySize.Size2048;
        var data = "Hello, World!"u8.ToArray();

        var encryptionService = mock.Create<EncryptionService>();

        var key = await encryptionService.GenerateKeyAsync((int)keySize);

        // Act
        var encryptedData1 = await encryptionService.EncryptAsync(data, key);
        var encryptedData2 = await encryptionService.EncryptAsync(data, key);

        // Assert
        Assert.NotEqual(encryptedData1, encryptedData2);
    }

    [Fact]
    public async Task DecryptAsync_ShouldReturnSameDecryptedData()
    {
        using var mock = AutoMock.GetLoose();

        // Arrange
        const KeySize keySize = KeySize.Size2048;
        var data = "Hello, World!"u8.ToArray();

        var encryptionService = mock.Create<EncryptionService>();

        var key = await encryptionService.GenerateKeyAsync((int)keySize);
        var encryptedData = await encryptionService.EncryptAsync(data, key);

        // Act
        var decryptedData1 = await encryptionService.DecryptAsync(encryptedData, key);
        var decryptedData2 = await encryptionService.DecryptAsync(encryptedData, key);

        // Assert
        Assert.Equal(decryptedData1, decryptedData2);
    }

    [Fact]
    public async Task EncryptAsync_DecryptAsync_PlainTextAfterDecryptShouldBeTheSame()
    {
        using var mock = AutoMock.GetLoose();

        // Arrange
        const KeySize keySize = KeySize.Size2048;
        var data = "Hello, World!"u8.ToArray();

        var encryptionService = mock.Create<EncryptionService>();

        var key = await encryptionService.GenerateKeyAsync((int)keySize);

        // Act
        var encryptedData = await encryptionService.EncryptAsync(data, key);
        var decryptedData = await encryptionService.DecryptAsync(encryptedData, key);

        // Assert
        Assert.Equal(data, decryptedData);
        testOutputHelper.WriteLine(Encoding.UTF8.GetString(decryptedData));
    }
}