using TicketFlow.Contexts.Sales.Domain.ValueObjects;

namespace TicketFlow.Contexts.Sales.Domain.Entities
{
    public class TemporaryReservation
    {
        public Guid Id { get; private set; }
        public Guid EventoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public QuantidadeIngressos Quantidade { get; private set; } // Reutilizando nosso VO!
        public PeriodoValidade Validade { get; private set; } // O novo VO encapsulando as datas

        // A entidade apenas repassa a pergunta para o VO, passando o contexto (UtcNow)
        public bool Ativa => Validade.EstaAtivo(DateTime.UtcNow);
        public TimeSpan TempoRestante => Validade.ObterTempoRestante(DateTime.UtcNow);

        protected TemporaryReservation() { }

        public TemporaryReservation(Guid eventoId, Guid clienteId, QuantidadeIngressos quantidade, int minutosExpiracao = 15)
        {
            // Novas validações de comportamento/estado para garantir integridade
            if (eventoId == Guid.Empty)
                throw new ArgumentException("O ID do evento é obrigatório para a reserva.");

            if (clienteId == Guid.Empty)
                throw new ArgumentException("O ID do cliente é obrigatório para a reserva.");

            Id = Guid.NewGuid();
            EventoId = eventoId;
            ClienteId = clienteId;
            Quantidade = quantidade; // O VO já garante que é > 0 e <= 10

            // Instanciamos o VO injetando o momento atual
            Validade = new PeriodoValidade(DateTime.UtcNow, minutosExpiracao);
        }
    }
}