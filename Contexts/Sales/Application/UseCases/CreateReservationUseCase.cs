using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Cria uma reserva temporária de ingressos (Should Have).
    /// O ingresso fica reservado por X minutos aguardando o pagamento.
    /// </summary>
    public class CreateReservationUseCase
    {
        private readonly IReservationRepository _reservationRepository;

        public CreateReservationUseCase(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<TemporaryReservation> ExecutarAsync(CreateReservationRequest request)
        {
            // Remove reservas expiradas antes de criar a nova
            await _reservationRepository.RemoverExpiradas();

            var reserva = new TemporaryReservation(
                request.EventoId,
                request.ClienteId,
                request.Quantidade,
                request.MinutosExpiracao);

            await _reservationRepository.SalvarAsync(reserva);
            return reserva;
        }
    }
}
