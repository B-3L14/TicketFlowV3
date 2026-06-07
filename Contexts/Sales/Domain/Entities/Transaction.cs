using TicketFlow.Contexts.Sales.Domain.Enums;
using TicketFlow.Contexts.Sales.Domain.ValueObjects; // Import dos VOs

namespace TicketFlow.Contexts.Sales.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public Guid PedidoId { get; private set; }
        public Dinheiro Valor { get; private set; } // Reutilizando nosso VO!
        public PaymentStatus Status { get; private set; }
        public TokenPagamento TokenPagamento { get; private set; } // Aplicando o novo VO
        public DateTime CriadoEm { get; private set; }
        public DateTime? ProcessadoEm { get; private set; }

        protected Transaction() { }

        public Transaction(Guid pedidoId, Dinheiro valor, TokenPagamento tokenPagamento)
        {
            if (pedidoId == Guid.Empty)
                throw new ArgumentException("O ID do pedido é obrigatório para iniciar a transação.");

            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            Valor = valor;
            TokenPagamento = tokenPagamento;
            Status = PaymentStatus.Pendente;
            CriadoEm = DateTime.UtcNow;
        }

        public void Aprovar()
        {
            // Validação de transição de estado vital para financeiro
            if (Status != PaymentStatus.Pendente)
                throw new InvalidOperationException($"Não é possível aprovar uma transação que está no status '{Status}'. Apenas transações pendentes podem ser aprovadas.");

            Status = PaymentStatus.Aprovado;
            ProcessadoEm = DateTime.UtcNow;
        }

        public void Recusar()
        {
            // Validação de transição de estado vital para financeiro
            if (Status != PaymentStatus.Pendente)
                throw new InvalidOperationException($"Não é possível recusar uma transação que está no status '{Status}'. Apenas transações pendentes podem ser recusadas.");

            Status = PaymentStatus.Recusado;
            ProcessadoEm = DateTime.UtcNow;
        }
    }
}