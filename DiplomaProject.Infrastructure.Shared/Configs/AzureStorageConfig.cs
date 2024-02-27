namespace DiplomaProject.Infrastructure.Shared.Configs;

public class AzureStorageConfig
{
    public string ConnectionString { get; set; }
    public string FileContainerName { get; set; }
    public int MaximumConcurrency { get; set; }
    public long MaximumTransferSizeInBytes { get; set; }
}