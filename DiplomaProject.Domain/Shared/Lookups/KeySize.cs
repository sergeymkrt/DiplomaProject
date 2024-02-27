using DiplomaProject.Domain.AggregatesModel.Keys;

namespace DiplomaProject.Domain.Shared.Lookups;

public class KeySize(int id, string name) : Enumeration(id, name)
{
    public virtual ICollection<Key> Keys { get; private set; } = [];
}