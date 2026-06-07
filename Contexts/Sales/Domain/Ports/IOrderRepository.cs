using TicketFlow.Contexts.Sales.Domain.Entities;

namespace TicketFlow.Contexts.Sales.Domain.Ports
{
    public interface IOrderRepository
    {
        Task SalvarAsync(Order pedido);
        Task<Order?> ObterPorIdAsync(Guid id);
        Task AtualizarAsync(Order pedido);
        Task<IEnumerable<Order>> ListarPorClienteAsync(Guid clienteId);
    }
}
