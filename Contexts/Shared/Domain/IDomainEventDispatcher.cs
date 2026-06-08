namespace TicketFlow.Shared.Domain;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : class, IDomainEvent;
}