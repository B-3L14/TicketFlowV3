using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class GetBatchesByEventUseCase(IBatchRepository batchRepository)
{
    public async Task<IEnumerable<BatchResponse>> ExecuteAsync(Guid eventId)
    {
        var batches = await batchRepository.GetByEventAsync(eventId);
        return batches.Select(CreateBatchUseCase.ToBatchResponse);
    }
}
