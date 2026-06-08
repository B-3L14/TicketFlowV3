namespace TicketFlow.Contexts.Events.Application.DTOs;

public record CreateOrganizerRequest(
    string Name,
    string Email,
    string Phone,
    string? Document = null,
    string? LogoUrl = null,
    string? Description = null
);

public record UpdateOrganizerRequest(
    string Name,
    string Email,
    string Phone,
    string? Document = null,
    string? LogoUrl = null,
    string? Description = null
);

public record OrganizerResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string? Document,
    string? LogoUrl,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
