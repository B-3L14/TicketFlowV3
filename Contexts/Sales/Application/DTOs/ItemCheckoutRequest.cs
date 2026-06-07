namespace TicketFlow.Contexts.Sales.Application.DTOs
{
    public record ItemCheckoutRequest(
        Guid EventoId,
        int Quantidade,
        decimal PrecoUnitario
    );
}
