using TicketFlow.Contexts.Sales.Domain.Entities;

namespace TicketFlow.Contexts.Sales.Domain.Ports
{
    public interface ITransactionRepository
    {
        Task SalvarAsync(Transaction transacao);
        Task<Transaction?> ObterPorIdAsync(Guid id);
    }
}
