namespace DiplomaProject.Application.DTOs.Files;

public class FileDto
{
    public long Id { get; set; }
    public bool IsDirectory { get; set; }
    public string FileName { get; set; }
    public string FileSize { get; set; }
    public string MimeType { get; set; }
}