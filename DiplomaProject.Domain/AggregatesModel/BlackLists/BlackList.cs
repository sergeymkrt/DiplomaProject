namespace DiplomaProject.Domain.AggregatesModel.BlackLists;

public class BlackList : Entity
{
    public string Token { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
}