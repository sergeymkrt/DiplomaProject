namespace DiplomaProject.Domain.Extensions;

public static class DataManipulationExtensions
{
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}