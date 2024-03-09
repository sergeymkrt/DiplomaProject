using DiplomaProject.Domain.Shared.Lookups;
using DiplomaProject.Packages.Extensions;

namespace DiplomaProject.Infrastructure.Persistence.Configurations.Lookups;

public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
{
    public void Configure(EntityTypeBuilder<PermissionType> builder)
    {
        builder.ToBaseEnumerationConfig();
        var enumItems = typeof(Domain.Enums.PermissionType).ToLookupEnumItemList();
        builder.HasData(enumItems.Select(ei => new PermissionType(ei.Value, ei.Name)));
    }
}