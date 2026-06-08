namespace TicketFlow.Contexts.Events.Application.DTOs;

public record CreateCategoryRequest(
    string Name,
    string? Description = null,
    string? IconUrl = null
);

public record UpdateCategoryRequest(
    string Name,
    string? Description = null,
    string? IconUrl = null
);

public record CategoryResponse(
    Guid Id,
    string Name,
    string? Description,
    string? IconUrl,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
