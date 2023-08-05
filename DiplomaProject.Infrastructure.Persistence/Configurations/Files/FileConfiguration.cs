namespace DiplomaProject.Infrastructure.Persistence.Configurations.Files;

public class FileConfiguration: IEntityTypeConfiguration<Domain.AggregatesModel.FileAggregate.File>
{
    public void Configure(EntityTypeBuilder<Domain.AggregatesModel.FileAggregate.File> builder)
    {
        builder.ToBaseEntityConfig();
        
        builder
            .Property(o => o.FilePath)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}