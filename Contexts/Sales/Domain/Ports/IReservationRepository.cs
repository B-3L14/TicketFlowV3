using TicketFlow.Contexts.Sales.Domain.Entities;

namespace TicketFlow.Contexts.Sales.Domain.Ports
{
    public interface IReservationRepository
    {
        Task SalvarAsync(TemporaryReservation reserva);
        Task<TemporaryReservation?> ObterPorIdAsync(Guid id);
        Task RemoverAsync(Guid id);
        Task RemoverExpiradas();
    }
}
