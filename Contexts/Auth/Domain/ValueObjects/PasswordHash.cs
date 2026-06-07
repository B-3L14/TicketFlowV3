namespace TicketFlow.Contexts.Auth.Domain.ValueObjects
{
    public record PasswordHash
    {
        public string Valor { get; init; }

        public PasswordHash(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("O hash da senha não pode ser vazio.");

            Valor = valor;
        }

        public static implicit operator string(PasswordHash p) => p.Valor;
        public static implicit operator PasswordHash(string p) => new PasswordHash(p);
    }
}