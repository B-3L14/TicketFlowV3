namespace TicketFlow.Contexts.Sales.Application.DTOs
{
    public record CheckoutRequest(
        Guid ClienteId,
        string TokenPagamento,
        List<ItemCheckoutRequest> Itens
);
}

