using DiplomaProject.Domain.AggregatesModel.Groups;

namespace DiplomaProject.Infrastructure.Persistence.Configurations;

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.ToTable("UserGroups");
        builder.HasKey(x => new { x.UserId, x.GroupId });
    }
}