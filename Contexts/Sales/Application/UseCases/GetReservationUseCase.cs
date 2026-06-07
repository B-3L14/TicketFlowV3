using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Consulta uma reserva e retorna seu status e tempo restante (Should Have).
    /// </summary>
    public class GetReservationUseCase
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationUseCase(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<TemporaryReservation> ExecutarAsync(Guid reservaId)
        {
            var reserva = await _reservationRepository.ObterPorIdAsync(reservaId)
                ?? throw new KeyNotFoundException("Reserva não encontrada.");

            return reserva;
        }
    }
}
