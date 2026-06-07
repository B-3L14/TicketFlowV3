using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;
using TicketFlow.Contexts.Sales.Infrastructure.Data;

namespace TicketFlow.Contexts.Sales.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SalesDbContext _context;

        public TransactionRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync(Transaction transacao)
        {
            await _context.Transacoes.AddAsync(transacao);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction?> ObterPorIdAsync(Guid id)
        {
            return await _context.Transacoes.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}