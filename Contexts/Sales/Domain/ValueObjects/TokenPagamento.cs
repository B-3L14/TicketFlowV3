namespace TicketFlow.Contexts.Sales.Domain.ValueObjects
{
    public record TokenPagamento
    {
        public string Valor { get; init; }

        public TokenPagamento(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("O token de pagamento não pode ser nulo ou vazio.");

            // Aqui você poderia adicionar validações mais específicas futuramente, 
            // como um Regex para validar o formato exato que o seu gateway de pagamento (Stripe, Mercado Pago, etc) retorna.

            Valor = valor;
        }

        public static implicit operator string(TokenPagamento t) => t.Valor;
        public static implicit operator TokenPagamento(string t) => new TokenPagamento(t);
    }
}