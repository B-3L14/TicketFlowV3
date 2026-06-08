namespace TicketFlow.Contexts.Events.Domain.ValueObjects;

public record Address
{
    public string Street { get; init; }
    public string Number { get; init; }
    public string Complement { get; init; }
    public string Neighborhood { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }
    public string Country { get; init; }

    public Address(
        string street,
        string number,
        string complement,
        string neighborhood,
        string city,
        string state,
        string zipCode,
        string country = "Brasil")
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Rua é obrigatória.", nameof(street));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Cidade é obrigatória.", nameof(city));
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("Estado é obrigatório.", nameof(state));
        if (string.IsNullOrWhiteSpace(zipCode))
            throw new ArgumentException("CEP é obrigatório.", nameof(zipCode));

        Street = street.Trim();
        Number = number?.Trim() ?? string.Empty;
        Complement = complement?.Trim() ?? string.Empty;
        Neighborhood = neighborhood?.Trim() ?? string.Empty;
        City = city.Trim();
        State = state.Trim();
        ZipCode = zipCode.Trim();
        Country = string.IsNullOrWhiteSpace(country) ? "Brasil" : country.Trim();
    }

    public string FullAddress =>
        $"{Street}, {Number}{(string.IsNullOrEmpty(Complement) ? "" : $" - {Complement}")}, {Neighborhood}, {City} - {State}, {ZipCode}";
}
