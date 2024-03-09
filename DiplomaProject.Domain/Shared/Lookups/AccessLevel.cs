using DiplomaProject.Domain.AggregatesModel.Groups;

namespace DiplomaProject.Domain.Shared.Lookups;

public class AccessLevel(int id, string name) : Enumeration(id, name)
{
    public virtual ICollection<Group> Groups { get; private set; } = [];
}