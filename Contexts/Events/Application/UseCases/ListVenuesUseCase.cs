using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class ListVenuesUseCase(IVenueRepository venueRepository)
{
    public async Task<IEnumerable<VenueResponse>> ExecuteAsync()
    {
        var venues = await venueRepository.GetAllAsync();
        return venues.Select(CreateVenueUseCase.ToResponse);
    }
}
