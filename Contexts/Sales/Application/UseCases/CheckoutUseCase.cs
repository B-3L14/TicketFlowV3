using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    public class CheckoutUseCase
    {
        private readonly IPaymentGateway _pagamentoGateway;
        private readonly IOrderRepository _pedidoRepository;

        public CheckoutUseCase(
            IPaymentGateway pagamentoGateway,
            IOrderRepository pedidoRepository)
        {
            _pagamentoGateway = pagamentoGateway;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<Order> ExecutarAsync(CheckoutRequest request)
        {
            var pedido = new Order(request.ClienteId);

            foreach (var item in request.Itens)
            {
                pedido.AdicionarItem(item.EventoId, item.Quantidade, item.PrecoUnitario);
            }

            bool pagamentoAprovado = await _pagamentoGateway.ProcessarPagamentoAsync(pedido, request.TokenPagamento);

            if (pagamentoAprovado)
            {
                pedido.MarcarComoPago();
            }
            else
            {
                pedido.Cancelar();
            }

            await _pedidoRepository.SalvarAsync(pedido);

            return pedido;
        }
    }
}
