using Microsoft.Extensions.DependencyInjection;
using TicketFlow.Shared.Application;
using TicketFlow.Shared.Domain;

namespace TicketFlow.Shared.Infrastructure;

public class DependencyInjectionEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    public async Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : class, IDomainEvent
    {
        // Localiza todos os manipuladores registrados para o tipo específico de evento
        var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(domainEvent);
        }
    }
}