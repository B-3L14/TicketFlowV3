using System.Text.RegularExpressions;

namespace TicketFlow.Contexts.Auth.Domain.ValueObjects
{
    public record Cpf
    {
        public string Valor { get; init; }

        public Cpf(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("O CPF não pode ser nulo ou vazio.");

            // Remove tudo que não for número (ex: 123.456.789-00 -> 12345678900)
            var apenasNumeros = Regex.Replace(valor, "[^0-9]", "");

            if (apenasNumeros.Length != 11)
                throw new ArgumentException("O CPF deve conter exatamente 11 dígitos numéricos.");

            // Opcional: Aqui você poderia adicionar o algoritmo real de validação dos dígitos verificadores do CPF

            Valor = apenasNumeros;
        }

        public static implicit operator string(Cpf c) => c.Valor;
        public static implicit operator Cpf(string c) => new Cpf(c);
    }
}