using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Entities;

namespace TicketFlow.Contexts.Events.Application.UseCases;

internal static class EventProjection
{
    internal static EventResponse ToResponse(Event e) => new(
        Id: e.Id,
        Name: e.Name,
        EventType: e.EventType,
        Description: e.Description,
        Date: e.Date,
        StartTime: e.StartTime,
        Capacity: e.Capacity,
        AvailableTickets: e.AvailableTickets,
        TotalSold: e.TotalSold,
        BasePrice: e.BasePrice.Amount,
        CurrentPrice: e.CurrentPrice.Amount,
        ImageUrl: e.ImageUrl,
        Status: e.Status,
        VenueId: e.VenueId,
        VenueName: e.Venue?.Name ?? string.Empty,
        OrganizerId: e.OrganizerId,
        OrganizerName: e.Organizer?.Name ?? string.Empty,
        CategoryId: e.CategoryId,
        CategoryName: e.Category?.Name ?? string.Empty,
        CreatedAt: e.CreatedAt,
        UpdatedAt: e.UpdatedAt,
        Batches: e.Batches.Select(CreateBatchUseCase.ToBatchResponse).ToList().AsReadOnly()
    );

    internal static EventSummaryResponse ToSummary(Event e) => new(
        Id: e.Id,
        Name: e.Name,
        EventType: e.EventType,
        Date: e.Date,
        StartTime: e.StartTime,
        AvailableTickets: e.AvailableTickets,
        CurrentPrice: e.CurrentPrice.Amount,
        ImageUrl: e.ImageUrl,
        Status: e.Status,
        VenueName: e.Venue?.Name ?? string.Empty,
        OrganizerName: e.Organizer?.Name ?? string.Empty,
        CategoryName: e.Category?.Name ?? string.Empty
    );
}
