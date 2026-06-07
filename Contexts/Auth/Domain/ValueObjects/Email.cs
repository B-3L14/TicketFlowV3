using System.Text.RegularExpressions;

namespace TicketFlow.Contexts.Auth.Domain.ValueObjects
{
    public record Email
    {
        public string Valor { get; init; }

        public Email(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("O e-mail não pode ser nulo ou vazio.");

            // Validação de formato básico de e-mail
            if (!Regex.IsMatch(valor, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("O formato do e-mail é inválido.");

            // Normaliza para minúsculas para facilitar buscas no banco
            Valor = valor.ToLowerInvariant();
        }

        // Operadores implícitos para facilitar a conversão
        public static implicit operator string(Email e) => e.Valor;
        public static implicit operator Email(string e) => new Email(e);
    }
}