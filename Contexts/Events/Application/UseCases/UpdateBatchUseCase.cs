using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class UpdateBatchUseCase(IBatchRepository batchRepository)
{
    public async Task<BatchResponse> ExecuteAsync(Guid batchId, UpdateBatchRequest request)
    {
        var batch = await batchRepository.GetByIdAsync(batchId)
            ?? throw new InvalidOperationException($"Lote '{batchId}' não encontrado.");

        var price = new TicketPrice(request.Price);
        batch.Update(request.Name, price, request.TotalTickets, request.SaleStartDate, request.SaleEndDate, request.Order);
        await batchRepository.UpdateAsync(batch);

        return CreateBatchUseCase.ToBatchResponse(batch);
    }
}
