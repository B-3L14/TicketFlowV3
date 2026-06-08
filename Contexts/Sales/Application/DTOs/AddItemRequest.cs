namespace TicketFlow.Contexts.Sales.Application.DTOs
{
    public record AddItemRequest(
        Guid EventoId,
        int Quantidade
    );
}