using TicketFlow.Contexts.Events.Domain.Enums;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Domain.Entities;

public class Batch
{
    public Guid Id { get; init; }
    public Guid EventId { get; private set; }
    public string Name { get; private set; }
    public TicketPrice Price { get; private set; }
    public int TotalTickets { get; private set; }
    public int SoldTickets { get; private set; }
    public DateTime SaleStartDate { get; private set; }
    public DateTime SaleEndDate { get; private set; }
    public BatchStatus Status { get; private set; }
    public int Order { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; private set; }

    private Batch() { }

    internal Batch(
        Guid eventId,
        string name,
        TicketPrice price,
        int totalTickets,
        DateTime saleStartDate,
        DateTime saleEndDate,
        int order)
    {
        if (eventId == Guid.Empty)
            throw new ArgumentException("O ID do Evento é obrigatório.", nameof(eventId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do lote é obrigatório.", nameof(name));
        if (price is null)
            throw new ArgumentNullException(nameof(price));
        if (totalTickets <= 0)
            throw new ArgumentException("O número total de ingressos deve ser maior que zero.", nameof(totalTickets));
        if (saleEndDate <= saleStartDate)
            throw new ArgumentException("A data de término deve ser posterior à data de início.", nameof(saleEndDate));

        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        EventId = eventId;
        Name = name.Trim();
        Price = price;
        TotalTickets = totalTickets;
        SoldTickets = 0;
        SaleStartDate = saleStartDate;
        SaleEndDate = saleEndDate;
        Order = order;
        Status = BatchStatus.Upcoming;
    }

    public static Batch Create(
        Guid eventId,
        string name,
        TicketPrice price,
        int totalTickets,
        DateTime saleStartDate,
        DateTime saleEndDate,
        int order = 1)
        => new Batch(eventId, name, price, totalTickets, saleStartDate, saleEndDate, order);

    public int AvailableTickets => TotalTickets - SoldTickets;
    public bool HasTicketsAvailable => AvailableTickets > 0;

    public void Update(string name, TicketPrice price, int totalTickets, DateTime saleStartDate, DateTime saleEndDate, int order)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do lote é obrigatório.", nameof(name));
        if (price is null)
            throw new ArgumentNullException(nameof(price));
        if (totalTickets <= 0)
            throw new ArgumentException("O número total de ingressos deve ser maior que zero.", nameof(totalTickets));
        if (totalTickets < SoldTickets)
            throw new InvalidOperationException($"Não é possível reduzir o total abaixo da quantidade vendida ({SoldTickets}).");
        if (saleEndDate <= saleStartDate)
            throw new ArgumentException("A data de término deve ser posterior à data de início.", nameof(saleEndDate));

        Name = name.Trim();
        Price = price;
        TotalTickets = totalTickets;
        SaleStartDate = saleStartDate;
        SaleEndDate = saleEndDate;
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReserveTickets(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));
        if (Status != BatchStatus.Active)
            throw new InvalidOperationException($"O lote '{Name}' não está ativo.");
        if (quantity > AvailableTickets)
            throw new InvalidOperationException($"Ingressos insuficientes. Disponíveis: {AvailableTickets}.");

        SoldTickets += quantity;

        if (AvailableTickets == 0)
            Status = BatchStatus.SoldOut;

        UpdatedAt = DateTime.UtcNow;
    }

    public void ReleaseTickets(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));

        SoldTickets = Math.Max(0, SoldTickets - quantity);

        if (Status == BatchStatus.SoldOut && AvailableTickets > 0)
            Status = BatchStatus.Active;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == BatchStatus.SoldOut)
            throw new InvalidOperationException("Não é possível ativar um lote esgotado.");

        Status = BatchStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Expire()
    {
        Status = BatchStatus.Expired;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RefreshStatus()
    {
        var now = DateTime.UtcNow;
        if (Status == BatchStatus.SoldOut) return;

        if (now < SaleStartDate)
            Status = BatchStatus.Upcoming;
        else if (now >= SaleStartDate && now <= SaleEndDate)
            Status = BatchStatus.Active;
        else
            Status = BatchStatus.Expired;

        UpdatedAt = DateTime.UtcNow;
    }
}
