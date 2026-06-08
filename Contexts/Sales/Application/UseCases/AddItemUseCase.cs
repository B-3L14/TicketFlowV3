using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    public class AddItemUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventCatalogService _eventCatalogService;

        public AddItemUseCase(IOrderRepository orderRepository, IEventCatalogService eventCatalogService)
        {
            _orderRepository = orderRepository;
            _eventCatalogService = eventCatalogService;
        }

        public async Task<Order> ExecutarAsync(Guid pedidoId, AddItemRequest request)
        {
            var pedido = await _orderRepository.ObterPorIdAsync(pedidoId)
                ?? throw new KeyNotFoundException("Pedido não encontrado.");

            // 🚀 A MÁGICA ACONTECE AQUI: Busca o preço real diretamente do catálogo!
            decimal precoReal = await _eventCatalogService.GetPriceAsync(request.EventoId);

            pedido.AdicionarItem(request.EventoId, request.Quantidade, precoReal);

            await _orderRepository.AtualizarAsync(pedido);
            return pedido;
        }
    }
}