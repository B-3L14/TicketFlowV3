using TicketFlow.Contexts.Events.Domain.Enums;

namespace TicketFlow.Contexts.Events.Application.DTOs;

public record CreateBatchRequest(
    Guid EventId,
    string Name,
    decimal Price,
    int TotalTickets,
    DateTime SaleStartDate,
    DateTime SaleEndDate,
    int Order = 1
);

public record UpdateBatchRequest(
    string Name,
    decimal Price,
    int TotalTickets,
    DateTime SaleStartDate,
    DateTime SaleEndDate,
    int Order
);

public record BatchResponse(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    int TotalTickets,
    int SoldTickets,
    int AvailableTickets,
    DateTime SaleStartDate,
    DateTime SaleEndDate,
    BatchStatus Status,
    int Order,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
