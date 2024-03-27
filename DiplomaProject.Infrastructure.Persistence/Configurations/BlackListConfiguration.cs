using DiplomaProject.Domain.AggregatesModel.BlackLists;

namespace DiplomaProject.Infrastructure.Persistence.Configurations;

public class BlackListConfiguration : IEntityTypeConfiguration<BlackList>
{
    public void Configure(EntityTypeBuilder<BlackList> builder)
    {
        builder.ToBaseEntityConfig();

        builder.Property(x => x.Token).IsRequired();
        builder.Property(x => x.ExpirationDate).IsRequired();

        builder.HasIndex(x => x.Token).IsUnique();
    }
}