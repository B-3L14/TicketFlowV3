using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class CreateBatchUseCase(IEventRepository eventRepository, IBatchRepository batchRepository)
{
    public async Task<BatchResponse> ExecuteAsync(CreateBatchRequest request)
    {
        var @event = await eventRepository.GetByIdAsync(request.EventId)
            ?? throw new InvalidOperationException($"Evento '{request.EventId}' não encontrado.");

        var price = new TicketPrice(request.Price);
        var batch = Batch.Create(
            request.EventId,
            request.Name,
            price,
            request.TotalTickets,
            request.SaleStartDate,
            request.SaleEndDate,
            request.Order);

        @event.AddBatch(batch);
        await batchRepository.AddAsync(batch);
        await eventRepository.UpdateAsync(@event);

        return ToBatchResponse(batch);
    }

    internal static BatchResponse ToBatchResponse(Domain.Entities.Batch b) => new(
        b.Id, b.EventId, b.Name, b.Price.Amount, b.TotalTickets, b.SoldTickets,
        b.AvailableTickets, b.SaleStartDate, b.SaleEndDate, b.Status, b.Order,
        b.CreatedAt, b.UpdatedAt);
}
