using TicketFlow.Contexts.Sales.Domain.Enums;
using TicketFlow.Contexts.Sales.Domain.ValueObjects; // Import do VO

namespace TicketFlow.Domain.Entities
{
    public class Ingresso
    {
        public Guid Id { get; private set; }
        public HashIngresso Hash { get; private set; } // O VO substitui a string primitiva
        public Guid UsuarioId { get; private set; }
        public Guid EventoId { get; private set; }
        public TicketStatus Status { get; private set; }

        // Construtor vazio para o Entity Framework
        protected Ingresso() { }

        internal Ingresso(Guid usuarioId, Guid eventoId)
        {
            // Validações de integridade dos IDs
            if (usuarioId == Guid.Empty) throw new ArgumentException("O ID do usuário é obrigatório.");
            if (eventoId == Guid.Empty) throw new ArgumentException("O ID do evento é obrigatório.");

            Id = Guid.NewGuid();
            Hash = HashIngresso.GerarNovo(); // Delegando a geração para o VO
            UsuarioId = usuarioId;
            EventoId = eventoId;
            Status = TicketStatus.Disponivel;
        }

        public void Cancelar()
        {
            // Validação de transição de estado
            if (Status == TicketStatus.Usado)
                throw new InvalidOperationException("Não é possível cancelar um ingresso que já foi utilizado.");

            if (Status == TicketStatus.Cancelado)
                return; // Já está cancelado, não faz nada (idempotência)

            Status = TicketStatus.Cancelado;
        }

        public void MarcarComoUsado()
        {
            // Validação de transição de estado
            if (Status != TicketStatus.Disponivel)
                throw new InvalidOperationException($"Não é possível utilizar um ingresso que está no status '{Status}'. Ele precisa estar Disponível.");

            Status = TicketStatus.Usado;
        }
    }
}