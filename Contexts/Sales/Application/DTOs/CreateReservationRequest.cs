namespace TicketFlow.Contexts.Sales.Application.DTOs
{
    public record CreateReservationRequest(
        Guid EventoId,
        Guid ClienteId,
        int Quantidade,
        int MinutosExpiracao = 15
    );
}
