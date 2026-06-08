using TicketFlow.Shared.Domain;

namespace TicketFlow.Shared.Application;

public interface IEventHandler<in TEvent> where TEvent : class, IDomainEvent
{
    Task HandleAsync(TEvent @event);
}