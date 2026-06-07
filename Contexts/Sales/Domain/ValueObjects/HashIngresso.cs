namespace TicketFlow.Contexts.Sales.Domain.ValueObjects
{
    public record HashIngresso
    {
        public string Valor { get; init; }

        // Construtor para carregar um hash existente (ex: vindo do banco de dados via EF)
        public HashIngresso(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || valor.Length != 10)
                throw new ArgumentException("O hash do ingresso deve conter exatamente 10 caracteres.");

            Valor = valor;
        }

        // Factory method para gerar um novo hash para novos ingressos
        public static HashIngresso GerarNovo()
        {
            var novoHash = Guid.NewGuid().ToString("N")[..10].ToUpper();
            return new HashIngresso(novoHash);
        }

        public static implicit operator string(HashIngresso h) => h.Valor;
    }
}