using DiplomaProject.Domain.AggregatesModel.Groups;
using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Domain.AggregatesModel.UserSessions;
using Microsoft.AspNetCore.Identity;
using AccessLevel = DiplomaProject.Domain.Shared.Lookups.AccessLevel;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Domain.Entities.User;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? SurName { get; set; }

    public int AccessLevelId { get; set; }
    public virtual AccessLevel AccessLevel { get; set; }

    public long PersonalDirectoryId { get; set; }
    public virtual Directory PersonalDirectory { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; } = [];
    public ICollection<File> Files { get; set; } = [];
    public ICollection<Key> Keys { get; set; } = [];
    public ICollection<Directory> OwnedDirectories { get; set; } = [];
    public ICollection<Group> OwnedGroups { get; set; } = [];
    public ICollection<UserSession> UserSessions { get; set; } = [];
}