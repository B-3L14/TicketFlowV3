using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class UpdateVenueUseCase(IVenueRepository venueRepository)
{
    public async Task<VenueResponse> ExecuteAsync(Guid id, UpdateVenueRequest request)
    {
        var venue = await venueRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Local '{id}' não encontrado.");

        var address = new Address(
            request.Street, request.Number, request.Complement,
            request.Neighborhood, request.City, request.State, request.ZipCode, request.Country);

        venue.Update(request.Name, address, request.Capacity, request.MapUrl, request.Description);
        await venueRepository.UpdateAsync(venue);
        return CreateVenueUseCase.ToResponse(venue);
    }
}
