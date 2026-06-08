using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class CreateVenueUseCase(IVenueRepository venueRepository)
{
    public async Task<VenueResponse> ExecuteAsync(CreateVenueRequest request)
    {
        var address = new Address(
            request.Street, request.Number, request.Complement,
            request.Neighborhood, request.City, request.State, request.ZipCode, request.Country);

        var venue = Venue.Create(request.Name, address, request.Capacity, request.MapUrl, request.Description);
        await venueRepository.AddAsync(venue);
        return ToResponse(venue);
    }

    internal static VenueResponse ToResponse(Domain.Entities.Venue v) => new(
        v.Id, v.Name, v.Capacity,
        new AddressResponse(v.Address.Street, v.Address.Number, v.Address.Complement, v.Address.Neighborhood, v.Address.City, v.Address.State, v.Address.ZipCode, v.Address.Country, v.Address.FullAddress),
        v.MapUrl, v.Description, v.IsActive, v.CreatedAt, v.UpdatedAt);
}
