using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Cria um pedido novo, sempre com status Pendente (Must Have).
    /// </summary>
    public class CreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> ExecutarAsync(CreateOrderRequest request)
        {
            var pedido = new Order(request.ClienteId);
            await _orderRepository.SalvarAsync(pedido);
            return pedido;
        }
    }
}
