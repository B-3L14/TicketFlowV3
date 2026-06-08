using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class DeleteBatchUseCase(IEventRepository eventRepository, IBatchRepository batchRepository)
{
    public async Task ExecuteAsync(Guid eventId, Guid batchId)
    {
        var @event = await eventRepository.GetByIdWithDetailsAsync(eventId)
            ?? throw new InvalidOperationException($"Evento '{eventId}' não encontrado.");

        @event.RemoveBatch(batchId);
        await batchRepository.DeleteAsync(batchId);
        await eventRepository.UpdateAsync(@event);
    }
}
