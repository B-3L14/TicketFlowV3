using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Adiciona um item (evento + quantidade) ao pedido (Must Have).
    /// </summary>
    public class AddItemUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public AddItemUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> ExecutarAsync(Guid pedidoId, AddItemRequest request)
        {
            var pedido = await _orderRepository.ObterPorIdAsync(pedidoId)
                ?? throw new KeyNotFoundException("Pedido não encontrado.");

            pedido.AdicionarItem(request.EventoId, request.Quantidade, request.PrecoUnitario);

            await _orderRepository.AtualizarAsync(pedido);
            return pedido;
        }
    }
}
