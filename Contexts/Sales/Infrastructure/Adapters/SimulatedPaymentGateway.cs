using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Infrastructure.Adapters
{
    /// <summary>
    /// Gateway de pagamento simulado (Must Have).
    /// Regras de simulação:
    ///   - Token "recusado" (case-insensitive) → pagamento recusado.
    ///   - Token nulo ou vazio                 → pagamento recusado.
    ///   - Valor total do pedido <= 0           → pagamento recusado.
    ///   - Todos os demais tokens válidos        → pagamento aprovado.
    /// </summary>
    public class SimulatedPaymentGateway : IPaymentGateway
    {
        public Task<bool> ProcessarPagamentoAsync(Order pedido, string tokenPagamento)
        {
            if (string.IsNullOrWhiteSpace(tokenPagamento))
                return Task.FromResult(false);

            if (tokenPagamento.Equals("recusado", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(false);

            if (pedido.ValorTotal <= 0)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }
    }
}
