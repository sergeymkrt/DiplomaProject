using DiplomaProject.Domain.Shared.Lookups;
using DiplomaProject.Packages.Extensions;

namespace DiplomaProject.Infrastructure.Persistence.Configurations.Lookups;

public class AccessLevelConfiguration : IEntityTypeConfiguration<AccessLevel>
{
    public void Configure(EntityTypeBuilder<AccessLevel> builder)
    {
        builder.ToBaseEnumerationConfig();

        var enumItems = typeof(Domain.Enums.AccessLevel).ToLookupEnumItemList();
        builder.HasData(enumItems.Select(ei => new AccessLevel(ei.Value, ei.Name)));
    }
}