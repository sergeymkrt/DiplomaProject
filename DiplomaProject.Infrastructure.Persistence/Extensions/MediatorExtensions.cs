namespace DiplomaProject.Infrastructure.Persistence.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, List<INotification> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}