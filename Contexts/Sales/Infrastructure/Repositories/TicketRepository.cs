using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Sales.Domain.Ports;
using TicketFlow.Contexts.Sales.Infrastructure.Data;
using TicketFlow.Domain.Entities;

namespace TicketFlow.Contexts.Sales.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly SalesDbContext _context;

        public TicketRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<Ingresso?> ObterPorHashAsync(string hash)
        {
            // O EF Core sabe lidar com a conversão da string para o Value Object HashIngresso
            // graças ao mapeamento que fizemos com HasConversion()
            return await _context.Ingressos.FirstOrDefaultAsync(i => i.Hash == hash);
        }

        public async Task<Ingresso?> ObterPorIdAsync(Guid id)
        {
            return await _context.Ingressos.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AtualizarAsync(Ingresso ingresso)
        {
            _context.Ingressos.Update(ingresso);
            await _context.SaveChangesAsync();
        }
    }
}