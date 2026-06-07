using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;
using TicketFlow.Contexts.Sales.Infrastructure.Data;

namespace TicketFlow.Contexts.Sales.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SalesDbContext _context;

        public OrderRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync(Order pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)     // Traz os itens junto
                .Include(p => p.Ingressos) // Traz os ingressos gerados
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AtualizarAsync(Order pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> ListarPorClienteAsync(Guid clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .Include(p => p.Ingressos)
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }
    }
}