using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DiplomaProject.Infrastructure.Shared.Configs;
using Microsoft.Extensions.Options;

namespace DiplomaProject.Infrastructure.Shared.ExternalServices;

public class AzureStorageManagementService(IOptions<AzureStorageConfig> config) : IFileManagementService
{
    public async Task<string> WriteFileAsync(Stream stream, string fileName)
    {
        var blobClient = await GetBlobClientAsync(fileName);

        var options = new BlobUploadOptions
        {
            TransferOptions = new StorageTransferOptions()
            {
                // Set the maximum number of workers that 
                // may be used in a parallel transfer.
                MaximumConcurrency = config.Value.MaximumConcurrency,
                MaximumTransferSize = config.Value.MaximumTransferSizeInBytes,
            },
        };

        stream.Seek(0, SeekOrigin.Begin);
        await blobClient.UploadAsync(stream, options, CancellationToken.None);

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<Stream> ReadFileAsync(string fileName)
    {
        var blobClient = await GetBlobClientAsync(fileName);

        if (!await blobClient.ExistsAsync())
        {
            return null;
        }

        return await blobClient.OpenReadAsync();
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var blobClient = await GetBlobClientAsync(fileName);
        await blobClient.DeleteIfExistsAsync();
    }

    #region private methods

    private async Task<BlobClient> GetBlobClientAsync(string fileName)
    {
        var blobServiceClient = new BlobServiceClient(config.Value.ConnectionString);

        var containerClient = blobServiceClient.GetBlobContainerClient(config.Value.FileContainerName);
        await containerClient.CreateIfNotExistsAsync();

        return containerClient.GetBlobClient(fileName);
    }

    #endregion
}