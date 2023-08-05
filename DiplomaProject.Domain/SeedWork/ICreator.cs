namespace DiplomaProject.Domain.SeedWork;

public interface ICreator
{
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public void SetCreator(string createdBy, DateTimeOffset createdDate)
    {
        CreatedBy = createdBy;
        CreatedDate = createdDate;
    }
}