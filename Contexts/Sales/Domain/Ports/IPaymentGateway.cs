using TicketFlow.Contexts.Sales.Domain.Entities;

namespace TicketFlow.Contexts.Sales.Domain.Ports
{
    public interface IPaymentGateway
    {
        Task<bool> ProcessarPagamentoAsync(Order pedido, string tokenPagamento);
    }
}
