namespace TicketFlow.Contexts.Sales.Domain.ValueObjects
{
    public record QuantidadeIngressos
    {
        public int Valor { get; init; }

        public QuantidadeIngressos(int valor)
        {
            if (valor <= 0)
                throw new ArgumentException("A quantidade de ingressos deve ser maior que zero.");

            // Nova validação inserida: Regra de negócio para evitar cambismo
            if (valor > 10)
                throw new ArgumentException("Não é permitido comprar mais de 10 ingressos por pedido.");

            Valor = valor;
        }

        public static implicit operator int(QuantidadeIngressos q) => q.Valor;
        public static implicit operator QuantidadeIngressos(int q) => new QuantidadeIngressos(q);
    }
}