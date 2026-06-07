using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Infrastructure.Adapters
{
    /// <summary>
    /// Adapter reservado para integração real com Mercado Pago.
    /// Não utilizado no fluxo principal — o SimulatedPaymentGateway cobre o Must Have.
    /// </summary>
    public class MercadoPagoAdapter : IPaymentGateway
    {
        public Task<bool> ProcessarPagamentoAsync(Order pedido, string tokenPagamento)
        {
            throw new NotImplementedException("Integração real com Mercado Pago não implementada nesta versão.");
        }
    }
}
