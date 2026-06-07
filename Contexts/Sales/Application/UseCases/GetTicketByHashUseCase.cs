using TicketFlow.Contexts.Sales.Domain.Ports;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Consulta o ingresso pelo hash único gerado após pagamento aprovado (Must Have).
    /// </summary>
    public class GetTicketByHashUseCase
    {
        private readonly ITicketRepository _ticketRepository;

        public GetTicketByHashUseCase(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Ingresso> ExecutarAsync(string hash)
        {
            var ingresso = await _ticketRepository.ObterPorHashAsync(hash)
                ?? throw new KeyNotFoundException("Ingresso não encontrado.");

            return ingresso;
        }
    }
}
