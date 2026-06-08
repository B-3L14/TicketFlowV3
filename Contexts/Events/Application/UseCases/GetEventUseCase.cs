using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class GetEventUseCase(IEventRepository eventRepository)
{
    public async Task<EventResponse> ExecuteAsync(Guid id)
    {
        var @event = await eventRepository.GetByIdWithDetailsAsync(id)
            ?? throw new InvalidOperationException($"Evento '{id}' não encontrado.");

        return EventProjection.ToResponse(@event);
    }
}
