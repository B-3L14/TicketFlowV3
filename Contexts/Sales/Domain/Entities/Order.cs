using TicketFlow.Contexts.Sales.Domain.Enums;
using TicketFlow.Contexts.Sales.Domain.ValueObjects;
using TicketFlow.Domain.Entities; // Import dos VOs

namespace TicketFlow.Contexts.Sales.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _itens = new();
        public IReadOnlyCollection<OrderItem> Itens => _itens.AsReadOnly();

        private readonly List<Ingresso> _ingressos = new();
        public IReadOnlyCollection<Ingresso> Ingressos => _ingressos.AsReadOnly();

        public decimal ValorTotal => _itens.Sum(i => i.ValorTotal);

        protected Order() { }

        public Order(Guid clienteId)
        {
            if (clienteId == Guid.Empty)
                throw new ArgumentException("O ID do cliente é obrigatório.", nameof(clienteId));

            Id = Guid.NewGuid();
            ClienteId = clienteId;
            Status = OrderStatus.Pendente;
        }

        // A assinatura do método agora exige nossos Value Objects!
        public void AdicionarItem(Guid eventoId, QuantidadeIngressos quantidade, Dinheiro precoUnitario)
        {
            if (Status != OrderStatus.Pendente)
                throw new InvalidOperationException("Não é possível alterar itens de um pedido que não está pendente.");

            _itens.Add(new OrderItem(eventoId, quantidade, precoUnitario));
        }

        public void RemoverItem(Guid itemId)
        {
            if (Status != OrderStatus.Pendente)
                throw new InvalidOperationException("Não é possível alterar itens de um pedido que não está pendente.");

            var item = _itens.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _itens.Remove(item);
            }
        }

        public void MarcarComoPago()
        {
            if (Status != OrderStatus.Pendente)
                throw new InvalidOperationException("Apenas pedidos pendentes podem ser pagos.");

            if (!_itens.Any())
                throw new InvalidOperationException("O pedido precisa ter pelo menos um item para ser pago.");

            Status = OrderStatus.Pago;
            GerarIngressos();
        }

        public void Cancelar()
        {
            if (Status == OrderStatus.Cancelado) return;

            Status = OrderStatus.Cancelado;

            foreach (var ingresso in _ingressos)
            {
                ingresso.Cancelar();
            }
        }

        private void GerarIngressos()
        {
            foreach (var item in _itens)
            {
                for (int i = 0; i < item.Quantidade.Valor; i++) // Lendo o valor do VO
                {
                    _ingressos.Add(new Ingresso(ClienteId, item.EventoId));
                }
            }
        }
    }

   
    
}