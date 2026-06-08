using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class ListEventsUseCase(IEventRepository eventRepository)
{
    public async Task<IEnumerable<EventSummaryResponse>> ExecuteAsync()
    {
        var events = await eventRepository.GetAllAsync();
        return events.Select(EventProjection.ToSummary);
    }
}
