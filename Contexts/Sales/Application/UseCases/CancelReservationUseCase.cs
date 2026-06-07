using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Cancela uma reserva antes do tempo, liberando o ingresso para o estoque (Should Have).
    /// </summary>
    public class CancelReservationUseCase
    {
        private readonly IReservationRepository _reservationRepository;

        public CancelReservationUseCase(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task ExecutarAsync(Guid reservaId)
        {
            var reserva = await _reservationRepository.ObterPorIdAsync(reservaId)
                ?? throw new KeyNotFoundException("Reserva não encontrada.");

            await _reservationRepository.RemoverAsync(reservaId);
        }
    }
}
