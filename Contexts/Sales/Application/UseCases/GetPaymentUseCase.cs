using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Application.UseCases
{
    /// <summary>
    /// Consulta o status de um pagamento (Must Have).
    /// </summary>
    public class GetPaymentUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetPaymentUseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> ExecutarAsync(Guid transacaoId)
        {
            var transacao = await _transactionRepository.ObterPorIdAsync(transacaoId)
                ?? throw new KeyNotFoundException("Transação não encontrada.");

            return transacao;
        }
    }
}
