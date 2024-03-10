using DiplomaProject.Domain.AggregatesModel.Groups;
using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Domain.Entities.User;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? SurName { get; set; }

    public int AccessLevelId { get; set; }
    public AccessLevel AccessLevel { get; set; }

    public long PersonalDirectoryId { get; set; }
    public Directory PersonalDirectory { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; } = [];
    public ICollection<File> Files { get; set; } = [];
    public ICollection<Key> Keys { get; set; } = [];
    public ICollection<Directory> OwnedDirectories { get; set; } = [];
    public ICollection<Group> OwnedGroups { get; set; } = [];
}