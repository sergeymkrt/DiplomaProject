namespace DiplomaProject.Domain.SeedWork;

public abstract class DomainEvent : INotification
{
    /// <summary>
    /// Specifies if event handler for this event will run after data persistence
    /// </summary>
    public bool IsPostEvent { get; private set; }

    public void SetIsPostEvent(bool isPostEvent)
    {
        IsPostEvent = isPostEvent;
    }
}
