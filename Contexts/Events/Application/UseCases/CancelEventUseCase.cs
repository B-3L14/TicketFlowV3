using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class CancelEventUseCase(IEventRepository eventRepository)
{
    public async Task<EventResponse> ExecuteAsync(Guid id)
    {
        var @event = await eventRepository.GetByIdWithDetailsAsync(id)
            ?? throw new InvalidOperationException($"Evento '{id}' não encontrado.");

        @event.Cancel();
        await eventRepository.UpdateAsync(@event);
        return EventProjection.ToResponse(@event);
    }
}
