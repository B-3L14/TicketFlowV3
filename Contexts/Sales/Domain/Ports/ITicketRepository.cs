using TicketFlow.Domain.Entities;

namespace TicketFlow.Contexts.Sales.Domain.Ports
{
    public interface ITicketRepository
    {
        Task<Ingresso?> ObterPorHashAsync(string hash);
        Task<Ingresso?> ObterPorIdAsync(Guid id);
        Task AtualizarAsync(Ingresso ingresso);
    }
}
