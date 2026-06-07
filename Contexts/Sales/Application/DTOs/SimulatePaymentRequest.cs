namespace TicketFlow.Contexts.Sales.Application.DTOs
{
    public record SimulatePaymentRequest(
        Guid PedidoId,
        string TokenPagamento
    );
}
