using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Remove um item específico do carrinho (Must Have).
    /// </summary>
    public class RemoveItemUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public RemoveItemUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task ExecutarAsync(Guid pedidoId, Guid itemId)
        {
            var pedido = await _orderRepository.ObterPorIdAsync(pedidoId)
                ?? throw new KeyNotFoundException("Pedido não encontrado.");

            pedido.RemoverItem(itemId);

            await _orderRepository.AtualizarAsync(pedido);
        }
    }
}
