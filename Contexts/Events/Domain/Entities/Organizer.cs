using TicketFlow.Contexts.Auth.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Domain.Entities;

public class Organizer
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string Phone { get; private set; }
    public string? Document { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<Event> _events = new();
    public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

    private Organizer() { }

    // 1. Construtor original (Gera um novo ID automático)
    internal Organizer(string name, Email email, string phone, string? document)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do organizador é obrigatório.", nameof(name));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Telefone do organizador é obrigatório.", nameof(phone));

        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Name = name.Trim();
        Email = email;
        Phone = phone.Trim();
        Document = document?.Trim();
        IsActive = true;
    }

    // 2. Novo construtor (Recebe o ID do evento de sincronização)
    internal Organizer(Guid id, string name, Email email, string phone, string? document)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do organizador é obrigatório.", nameof(name));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Telefone do organizador é obrigatório.", nameof(phone));

        Id = id;
        CreatedAt = DateTime.UtcNow;
        Name = name.Trim();
        Email = email;
        Phone = phone.Trim();
        Document = document?.Trim();
        IsActive = true;
    }

    // Factory Method original (usado pelo CreateOrganizerUseCase)
    public static Organizer Create(string name, Email email, string phone, string? document = null)
        => new Organizer(name, email, phone, document);

    // Factory Method novo (usado pelo UserRegisteredHandler)
    public static Organizer CreateWithId(Guid id, string name, Email email, string phone, string? document = null)
        => new Organizer(id, name, email, phone, document);


    public void Update(string name, Email email, string phone, string? document, string? logoUrl, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do organizador é obrigatório.", nameof(name));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Telefone do organizador é obrigatório.", nameof(phone));

        Name = name.Trim();
        Email = email;
        Phone = phone.Trim();
        Document = document?.Trim();
        LogoUrl = logoUrl?.Trim();
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
