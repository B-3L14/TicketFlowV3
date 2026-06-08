namespace TicketFlow.Contexts.Events.Domain.ValueObjects;

public record TicketPrice
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public TicketPrice(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new ArgumentException("Preço não pode ser negativo.", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Moeda é obrigatória.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant().Trim();
    }

    public bool IsFree => Amount == 0;

    public override string ToString() =>
        IsFree ? "Gratuito" : $"{Currency} {Amount:F2}";

    public static implicit operator decimal(TicketPrice p) => p.Amount;
    public static implicit operator TicketPrice(decimal amount) => new TicketPrice(amount);
}
