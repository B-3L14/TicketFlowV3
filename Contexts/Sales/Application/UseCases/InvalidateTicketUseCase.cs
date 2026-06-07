using TicketFlow.Contexts.Sales.Domain.Ports;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Invalida (cancela) um ingresso específico (Should Have).
    /// Usado quando o evento é cancelado pelo organizador.
    /// </summary>
    public class InvalidateTicketUseCase
    {
        private readonly ITicketRepository _ticketRepository;

        public InvalidateTicketUseCase(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Ingresso> ExecutarAsync(Guid ingressoId)
        {
            var ingresso = await _ticketRepository.ObterPorIdAsync(ingressoId)
                ?? throw new KeyNotFoundException("Ingresso não encontrado.");

            ingresso.Cancelar();
            await _ticketRepository.AtualizarAsync(ingresso);

            return ingresso;
        }
    }
}
