using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Domain.Entities;

public class Venue
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public int Capacity { get; private set; }
    public string? MapUrl { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<Event> _events = new();
    public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

    private Venue() { }

    internal Venue(string name, Address address, int capacity, string? mapUrl, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do local é obrigatório.", nameof(name));
        if (address is null)
            throw new ArgumentNullException(nameof(address), "Endereço é obrigatório.");
        if (capacity <= 0)
            throw new ArgumentException("A capacidade deve ser maior que zero.", nameof(capacity));

        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Name = name.Trim();
        Address = address;
        Capacity = capacity;
        MapUrl = mapUrl?.Trim();
        Description = description?.Trim();
        IsActive = true;
    }

    public static Venue Create(string name, Address address, int capacity, string? mapUrl = null, string? description = null)
        => new Venue(name, address, capacity, mapUrl, description);

    public void Update(string name, Address address, int capacity, string? mapUrl, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do local é obrigatório.", nameof(name));
        if (capacity <= 0)
            throw new ArgumentException("A capacidade deve ser maior que zero.", nameof(capacity));

        Name = name.Trim();
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Capacity = capacity;
        MapUrl = mapUrl?.Trim();
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
