using DiplomaProject.Packages.Extensions;

namespace DiplomaProject.Infrastructure.Persistence.Extensions;

public static class EntityConfigurationExtensions
{
    public static void ToBaseEntityConfig<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        bool includeDates = true,
        bool includeUsers = true) where TEntity : Entity
    {
        builder.ToTable(typeof(TEntity).Name.Pluralize().ToSnakeCase());
        builder.HasKey(b => b.Id);
        builder.Ignore(b => b.DomainEvents);

        builder.Property(b => b.Id)
            .HasColumnName("id");

        builder.Property(b => b.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(250)
            //TEMP
            .HasDefaultValue("testId");

        builder.Property(b => b.CreatedDate)
            .HasColumnName("created_date")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.ModifiedBy)
            .HasColumnName("modified_by")
            .HasMaxLength(250);

        builder.Property(b => b.ModifiedDate)
            .HasColumnName("modified_date")
            .HasDefaultValueSql("GETUTCDATE()");

        if (!includeUsers)
        {
            builder.Ignore(b => b.CreatedBy);
            builder.Ignore(b => b.ModifiedBy);
        }

        if (!includeDates)
        {
            builder.Ignore(b => b.CreatedDate);
            builder.Ignore(b => b.ModifiedDate);
        }

    }

    public static void ToBaseEnumerationConfig<TEnum>(this EntityTypeBuilder<TEnum> builder) where TEnum : Enumeration
    {
        builder.ToTable(typeof(TEnum).Name.Pluralize().ToSnakeCase());

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .IsRequired();

        builder.Property(o => o.Name)
            .HasMaxLength(250)
            .HasColumnName("name")
            .IsRequired();
    }
}