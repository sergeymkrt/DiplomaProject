using DiplomaProject.Domain.Entities.User;

namespace DiplomaProject.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.Name)
            .HasDefaultValue(null)
            .IsRequired(false);

        builder.Property(p => p.SurName)
            .HasDefaultValue(null)
            .IsRequired(false);

        builder.HasMany(x => x.UserGroups)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.UserSessions)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}