using DiplomaProject.Domain.AggregatesModel.Groups;

namespace DiplomaProject.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToBaseEntityConfig();

        builder.HasMany(x => x.UserGroups)
            .WithOne(x => x.Group)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AccessLevel)
            .WithMany(x => x.Groups)
            .HasForeignKey(x => x.AccessLevelId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Owner)
            .WithMany(x => x.OwnedGroups)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Directory)
            .WithOne(x => x.Group)
            .HasForeignKey<Group>(x => x.DirectoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}