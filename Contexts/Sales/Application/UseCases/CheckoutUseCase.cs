using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    public class CheckoutUseCase
    {
        private readonly IPaymentGateway _pagamentoGateway;
        private readonly IOrderRepository _pedidoRepository;
        private readonly IEventCatalogService _eventCatalogService;

        public CheckoutUseCase(
            IPaymentGateway pagamentoGateway,
            IOrderRepository pedidoRepository,
            IEventCatalogService eventCatalogService)
        {
            _pagamentoGateway = pagamentoGateway;
            _pedidoRepository = pedidoRepository;
            _eventCatalogService = eventCatalogService;
        }

        public async Task<Order> ExecutarAsync(CheckoutRequest request)
        {
            var pedido = new Order(request.ClienteId);

            foreach (var item in request.Itens)
            {
                // Busca o preço real
                decimal precoReal = await _eventCatalogService.GetPriceAsync(item.EventoId);
                pedido.AdicionarItem(item.EventoId, item.Quantidade, precoReal);
            }

            bool pagamentoAprovado = await _pagamentoGateway.ProcessarPagamentoAsync(pedido, request.TokenPagamento);

            if (pagamentoAprovado)
                pedido.MarcarComoPago();
            else
                pedido.Cancelar();

            await _pedidoRepository.SalvarAsync(pedido);

            return pedido;
        }
    }
}