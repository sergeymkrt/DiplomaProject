namespace DiplomaProject.Application.DTOs.Directories;

public class DirectoryDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long? ParentDirectoryId { get; set; }
}