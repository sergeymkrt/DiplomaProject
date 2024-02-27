using DiplomaProject.Domain.Shared.Lookups;
using DiplomaProject.Packages.Extensions;

namespace DiplomaProject.Infrastructure.Persistence.Configurations.Lookups;

public class KeySizeConfiguration : IEntityTypeConfiguration<KeySize>
{
    public void Configure(EntityTypeBuilder<KeySize> builder)
    {
        builder.ToBaseEnumerationConfig();

        var enumItems = typeof(Domain.Enums.KeySize).ToLookupEnumItemList();
        builder.HasData(enumItems.Select(ei => new KeySize(ei.Value, ei.Name)));
    }
}