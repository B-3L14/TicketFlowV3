using TicketFlow.Contexts.Events.Domain.Enums;

namespace TicketFlow.Contexts.Events.Application.DTOs;

public record CreateEventRequest(
    string Name,
    EventType EventType,
    string Description,
    DateTime Date,
    TimeSpan StartTime,
    int Capacity,
    decimal BasePrice,
    Guid VenueId,
    Guid OrganizerId,
    Guid CategoryId,
    string? ImageUrl = null
);

public record UpdateEventRequest(
    string Name,
    EventType EventType,
    string Description,
    DateTime Date,
    TimeSpan StartTime,
    int Capacity,
    decimal BasePrice,
    Guid VenueId,
    Guid OrganizerId,
    Guid CategoryId,
    string? ImageUrl = null
);

public record EventResponse(
    Guid Id,
    string Name,
    EventType EventType,
    string Description,
    DateTime Date,
    TimeSpan StartTime,
    int Capacity,
    int AvailableTickets,
    int TotalSold,
    decimal BasePrice,
    decimal CurrentPrice,
    string? ImageUrl,
    EventStatus Status,
    Guid VenueId,
    string VenueName,
    Guid OrganizerId,
    string OrganizerName,
    Guid CategoryId,
    string CategoryName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyCollection<BatchResponse> Batches
);

public record EventSummaryResponse(
    Guid Id,
    string Name,
    EventType EventType,
    DateTime Date,
    TimeSpan StartTime,
    int AvailableTickets,
    decimal CurrentPrice,
    string? ImageUrl,
    EventStatus Status,
    string VenueName,
    string OrganizerName,
    string CategoryName
);
