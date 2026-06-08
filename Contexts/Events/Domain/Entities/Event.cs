using TicketFlow.Contexts.Events.Domain.Enums;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Domain.Entities;

public class Event
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public EventType EventType { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public int Capacity { get; private set; }
    public TicketPrice BasePrice { get; private set; }
    public string? ImageUrl { get; private set; }
    public EventStatus Status { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; private set; }

    public Guid VenueId { get; private set; }
    public Venue Venue { get; private set; } = null!;

    public Guid OrganizerId { get; private set; }
    public Organizer Organizer { get; private set; } = null!;

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    private readonly List<Batch> _batches = new();
    public IReadOnlyCollection<Batch> Batches => _batches.AsReadOnly();

    private Event() { }

    internal Event(
        string name,
        EventType eventType,
        string description,
        DateTime date,
        TimeSpan startTime,
        int capacity,
        TicketPrice basePrice,
        Guid venueId,
        Guid organizerId,
        Guid categoryId,
        string? imageUrl)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Status = EventStatus.Draft;
        ValidateAndSet(name, eventType, description, date, startTime, capacity, basePrice, venueId, organizerId, categoryId, imageUrl);
    }

    public static Event Create(
        string name,
        EventType eventType,
        string description,
        DateTime date,
        TimeSpan startTime,
        int capacity,
        TicketPrice basePrice,
        Guid venueId,
        Guid organizerId,
        Guid categoryId,
        string? imageUrl = null)
        => new Event(name, eventType, description, date, startTime, capacity, basePrice, venueId, organizerId, categoryId, imageUrl);

    public int TotalSold => _batches.Sum(b => b.SoldTickets);
    public int AvailableTickets => Math.Max(0, Capacity - TotalSold);

    public bool IsAvailable =>
        Status == EventStatus.Published &&
        AvailableTickets > 0 &&
        Date > DateTime.UtcNow;

    public Batch? ActiveBatch =>
        _batches.FirstOrDefault(b => b.Status == BatchStatus.Active && b.HasTicketsAvailable);

    public TicketPrice CurrentPrice =>
        ActiveBatch?.Price ?? BasePrice;

    public void Publish()
    {
        if (Status != EventStatus.Draft)
            throw new InvalidOperationException("Apenas eventos em rascunho podem ser publicados.");
        if (!_batches.Any())
            throw new InvalidOperationException("Não é possível publicar um evento sem pelo menos um lote.");
        if (Date <= DateTime.UtcNow)
            throw new InvalidOperationException("Não é possível publicar um evento com data passada.");

        Status = EventStatus.Published;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == EventStatus.Cancelled)
            throw new InvalidOperationException("O evento já está cancelado.");
        if (Status == EventStatus.Finished)
            throw new InvalidOperationException("Não é possível cancelar um evento finalizado.");

        Status = EventStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Finish()
    {
        if (Status != EventStatus.Published)
            throw new InvalidOperationException("Apenas eventos publicados podem ser finalizados.");

        Status = EventStatus.Finished;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Revert()
    {
        if (Status != EventStatus.Published)
            throw new InvalidOperationException("Apenas eventos publicados podem ser revertidos para rascunho.");

        Status = EventStatus.Draft;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string name,
        EventType eventType,
        string description,
        DateTime date,
        TimeSpan startTime,
        int capacity,
        TicketPrice basePrice,
        Guid venueId,
        Guid organizerId,
        Guid categoryId,
        string? imageUrl)
    {
        if (Status == EventStatus.Cancelled)
            throw new InvalidOperationException("Não é possível atualizar um evento cancelado.");
        if (Status == EventStatus.Finished)
            throw new InvalidOperationException("Não é possível atualizar um evento finalizado.");

        ValidateAndSet(name, eventType, description, date, startTime, capacity, basePrice, venueId, organizerId, categoryId, imageUrl);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("URL da imagem é obrigatória.", nameof(imageUrl));

        ImageUrl = imageUrl.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddBatch(Batch batch)
    {
        if (batch is null) throw new ArgumentNullException(nameof(batch));
        if (Status == EventStatus.Cancelled)
            throw new InvalidOperationException("Não é possível adicionar lotes a um evento cancelado.");

        _batches.Add(batch);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveBatch(Guid batchId)
    {
        var batch = _batches.FirstOrDefault(b => b.Id == batchId)
            ?? throw new InvalidOperationException($"Lote {batchId} não encontrado.");

        if (batch.SoldTickets > 0)
            throw new InvalidOperationException("Não é possível remover um lote que já possui ingressos vendidos.");

        _batches.Remove(batch);
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateAndSet(
        string name, EventType eventType, string description,
        DateTime date, TimeSpan startTime, int capacity,
        TicketPrice basePrice, Guid venueId, Guid organizerId, Guid categoryId,
        string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do evento é obrigatório.", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Descrição do evento é obrigatória.", nameof(description));
        if (capacity <= 0)
            throw new ArgumentException("A capacidade deve ser maior que zero.", nameof(capacity));
        if (basePrice is null)
            throw new ArgumentNullException(nameof(basePrice));
        if (venueId == Guid.Empty)
            throw new ArgumentException("VenueId é obrigatório.", nameof(venueId));
        if (organizerId == Guid.Empty)
            throw new ArgumentException("OrganizerId é obrigatório.", nameof(organizerId));
        if (categoryId == Guid.Empty)
            throw new ArgumentException("CategoryId é obrigatório.", nameof(categoryId));

        Name = name.Trim();
        EventType = eventType;
        Description = description.Trim();
        Date = date;
        StartTime = startTime;
        Capacity = capacity;
        BasePrice = basePrice;
        VenueId = venueId;
        OrganizerId = organizerId;
        CategoryId = categoryId;
        ImageUrl = imageUrl?.Trim();
    }
}
