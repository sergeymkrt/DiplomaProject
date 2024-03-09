using DiplomaProject.Domain.AggregatesModel.Groups;
using DiplomaProject.Domain.Entities.User;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Domain.AggregatesModel.Directories;

public class Directory : Entity, IAggregateRoot
{
    public Directory()
    {

    }
    public Directory(string name, string ownerId, long? parentDirectoryId)
    {
        Name = name;
        OwnerId = ownerId;
        ParentDirectoryId = parentDirectoryId;
    }

    public string Name { get; set; }

    public long? ParentDirectoryId { get; set; }
    public virtual Directory ParentDirectory { get; set; }

    public virtual User PersonalOwner { get; set; }

    public string? OwnerId { get; set; }
    public virtual User Owner { get; set; }

    public virtual Group Group { get; set; }

    public ICollection<Directory> Directories { get; set; } = [];
    public ICollection<File> Files { get; set; } = [];
}