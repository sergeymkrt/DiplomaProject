using DiplomaProject.Domain.Entities.User;
using Directory = DiplomaProject.Domain.AggregatesModel.Directories.Directory;

namespace DiplomaProject.Infrastructure.Persistence.Configurations;

public class DirectoryConfiguration : IEntityTypeConfiguration<Directory>
{
    public void Configure(EntityTypeBuilder<Directory> builder)
    {
        builder.ToBaseEntityConfig();

        builder.HasMany(x => x.Files)
            .WithOne(x => x.Directory)
            .HasForeignKey(x => x.DirectoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ParentDirectory)
            .WithMany(x => x.Directories)
            .HasForeignKey(x => x.ParentDirectoryId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.PersonalOwner)
            .WithOne(x => x.PersonalDirectory)
            .HasForeignKey<User>(x => x.PersonalDirectoryId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Owner)
            .WithMany(x => x.OwnedDirectories)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Group)
            .WithOne(x => x.Directory)
            .HasForeignKey<Directory>(x => x.Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}