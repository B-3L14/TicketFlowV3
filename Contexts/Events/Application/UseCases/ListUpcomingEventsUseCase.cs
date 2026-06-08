using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class ListUpcomingEventsUseCase(IEventRepository eventRepository)
{
    public async Task<IEnumerable<EventSummaryResponse>> ExecuteAsync(int page = 1, int pageSize = 20)
    {
        var events = await eventRepository.GetUpcomingAsync(page, pageSize);
        return events.Select(EventProjection.ToSummary);
    }
}
