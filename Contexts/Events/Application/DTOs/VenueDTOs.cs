namespace TicketFlow.Contexts.Events.Application.DTOs;

public record CreateVenueRequest(
    string Name,
    int Capacity,
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country = "Brasil",
    string? MapUrl = null,
    string? Description = null
);

public record UpdateVenueRequest(
    string Name,
    int Capacity,
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country = "Brasil",
    string? MapUrl = null,
    string? Description = null
);

public record AddressResponse(
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country,
    string FullAddress
);

public record VenueResponse(
    Guid Id,
    string Name,
    int Capacity,
    AddressResponse Address,
    string? MapUrl,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
