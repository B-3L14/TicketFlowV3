using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class GetVenueUseCase(IVenueRepository venueRepository)
{
    public async Task<VenueResponse> ExecuteAsync(Guid id)
    {
        var venue = await venueRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Local '{id}' não encontrado.");

        return CreateVenueUseCase.ToResponse(venue);
    }
}
