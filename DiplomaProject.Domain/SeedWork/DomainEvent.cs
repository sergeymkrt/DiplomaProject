namespace DiplomaProject.Domain.SeedWork;

public abstract class DomainEvent : INotification
{
}

public abstract class PreDomainEvent : DomainEvent
{

}

public abstract class PostDomainEvent : DomainEvent
{

}
