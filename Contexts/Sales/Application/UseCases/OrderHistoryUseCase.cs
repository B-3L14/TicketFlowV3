using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Retorna o histórico de compras do cliente com os ingressos adquiridos (Should Have).
    /// </summary>
    public class OrderHistoryUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderHistoryUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> ExecutarAsync(Guid clienteId)
        {
            return await _orderRepository.ListarPorClienteAsync(clienteId);
        }
    }
}
