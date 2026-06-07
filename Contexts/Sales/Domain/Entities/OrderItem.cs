using TicketFlow.Contexts.Sales.Domain.ValueObjects;

namespace TicketFlow.Contexts.Sales.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid EventoId { get; private set; }
        public QuantidadeIngressos Quantidade { get; private set; }
        public Dinheiro PrecoUnitario { get; private set; }

        public Dinheiro ValorTotal => Quantidade.Valor * PrecoUnitario.Valor;

        protected OrderItem() { }

        // O construtor agora exige os Value Objects, eliminando a obsessão por primitivos.
        internal OrderItem(Guid eventoId, QuantidadeIngressos quantidade, Dinheiro precoUnitario)
        {
            if (eventoId == Guid.Empty)
                throw new ArgumentException("O ID do evento é obrigatório para o item do pedido.");

            Id = Guid.NewGuid();
            EventoId = eventoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }
    }
}