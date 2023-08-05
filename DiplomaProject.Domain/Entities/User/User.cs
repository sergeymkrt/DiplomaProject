using Microsoft.AspNetCore.Identity;
using File = DiplomaProject.Domain.AggregatesModel.FileAggregate.File;

namespace DiplomaProject.Domain.Entities.User;

public class User: IdentityUser
{
    public ICollection<File> Files { get; set; } = new List<File>();
}