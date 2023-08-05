namespace DiplomaProject.Domain.SeedWork;

public interface IModifier
{
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }

    public void SetModifier(string? modifiedBy, DateTimeOffset? modifiedDate)
    {
        ModifiedBy = modifiedBy;
        ModifiedDate = modifiedDate;
    }
}