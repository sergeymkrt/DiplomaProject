using Microsoft.AspNetCore.Identity;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Domain.Entities.User;

public class User: IdentityUser
{
    public string? Name { get; set; }
    public string? SurName { get; set; }
    public ICollection<File> Files { get; set; } = new List<File>();
}