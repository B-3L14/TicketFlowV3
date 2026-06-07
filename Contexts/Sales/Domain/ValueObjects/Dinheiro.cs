namespace TicketFlow.Contexts.Sales.Domain.ValueObjects
{
    public record Dinheiro
    {
        public decimal Valor { get; init; }

        public Dinheiro(decimal valor)
        {
            if (valor < 0)
                throw new ArgumentException("O valor monetário não pode ser negativo.");

            Valor = valor;
        }
        private Dinheiro() { }

        // Operadores implícitos facilitam a conversão no código
        public static implicit operator decimal(Dinheiro d) => d.Valor;
        public static implicit operator Dinheiro(decimal d) => new Dinheiro(d);
    }
}