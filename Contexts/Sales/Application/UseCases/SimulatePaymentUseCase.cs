using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Enums;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Simula o processamento de pagamento e consolida o pedido (Must Have).
    /// Controle de concorrência ativado aqui via lock no repositório.
    /// Um ingresso só é "Vendido" (gerado) se o pagamento for aprovado.
    /// </summary>
    public class SimulatePaymentUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPaymentGateway _paymentGateway;

        public SimulatePaymentUseCase(
            IOrderRepository orderRepository,
            ITransactionRepository transactionRepository,
            IPaymentGateway paymentGateway)
        {
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _paymentGateway = paymentGateway;
        }

        public async Task<(Transaction transacao, Order pedido)> ExecutarAsync(SimulatePaymentRequest request)
        {
            var pedido = await _orderRepository.ObterPorIdAsync(request.PedidoId)
                ?? throw new KeyNotFoundException("Pedido não encontrado.");

            if (pedido.Status != OrderStatus.Pendente)
                throw new InvalidOperationException("Apenas pedidos com status Pendente podem ser processados.");

            // Cria a transação de pagamento
            var transacao = new Transaction(pedido.Id, pedido.ValorTotal, request.TokenPagamento);

            // Processa via gateway (simulado) — controle de concorrência no repositório
            bool aprovado = await _paymentGateway.ProcessarPagamentoAsync(pedido, request.TokenPagamento);

            if (aprovado)
            {
                transacao.Aprovar();
                pedido.MarcarComoPago(); // Ingressos gerados apenas aqui (Must Have)
            }
            else
            {
                transacao.Recusar();
                pedido.Cancelar();
            }

            await _transactionRepository.SalvarAsync(transacao);
            await _orderRepository.AtualizarAsync(pedido);

            return (transacao, pedido);
        }
    }
}
