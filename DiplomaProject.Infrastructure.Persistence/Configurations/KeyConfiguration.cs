using DiplomaProject.Domain.AggregatesModel.Keys;

namespace DiplomaProject.Infrastructure.Persistence.Configurations;

public class KeyConfiguration : IEntityTypeConfiguration<Key>
{
    public void Configure(EntityTypeBuilder<Key> builder)
    {
        builder.ToBaseEntityConfig();

        builder.Property(x => x.PrivateKey)
            .HasColumnType("nvarchar(MAX)");

        builder.Property(x => x.PublicKey)
            .HasColumnType("nvarchar(MAX)");

        builder.HasOne(x => x.KeySize)
            .WithMany(x => x.Keys)
            .HasForeignKey(x => x.KeySizeID)
            .OnDelete(DeleteBehavior.NoAction);
    }
}