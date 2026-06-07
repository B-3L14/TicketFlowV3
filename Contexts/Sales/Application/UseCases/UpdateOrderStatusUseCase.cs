using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Enums;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Transita o pedido entre status válidos da máquina de estados (Must Have).
    /// Transições permitidas:
    ///   Pendente → Pago      (via pagamento confirmado)
    ///   Pendente → Cancelado (cancelamento manual)
    ///   Pago     → Cancelado (estorno / cancelamento pós-compra)
    /// </summary>
    public class UpdateOrderStatusUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> ExecutarAsync(Guid pedidoId, UpdateOrderStatusRequest request)
        {
            var pedido = await _orderRepository.ObterPorIdAsync(pedidoId)
                ?? throw new KeyNotFoundException("Pedido não encontrado.");

            if (!Enum.TryParse<OrderStatus>(request.Status, ignoreCase: true, out var novoStatus))
                throw new ArgumentException($"Status '{request.Status}' inválido. Valores aceitos: Pago, Cancelado.");

            switch (novoStatus)
            {
                case OrderStatus.Pago:
                    pedido.MarcarComoPago();
                    break;
                case OrderStatus.Cancelado:
                    pedido.Cancelar();
                    break;
                default:
                    throw new InvalidOperationException("Transição de status não permitida.");
            }

            await _orderRepository.AtualizarAsync(pedido);
            return pedido;
        }
    }
}
