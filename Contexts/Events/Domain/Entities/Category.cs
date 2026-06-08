namespace TicketFlow.Contexts.Events.Domain.Entities;

public class Category
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? IconUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<Event> _events = new();
    public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

    private Category() { }

    internal Category(string name, string? description, string? iconUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da categoria é obrigatório.", nameof(name));

        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Name = name.Trim();
        Description = description?.Trim();
        IconUrl = iconUrl?.Trim();
        IsActive = true;
    }

    public static Category Create(string name, string? description = null, string? iconUrl = null)
        => new Category(name, description, iconUrl);

    public void Update(string name, string? description, string? iconUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome da categoria é obrigatório.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
        IconUrl = iconUrl?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
