using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class ListOrganizersUseCase(IOrganizerRepository organizerRepository)
{
    public async Task<IEnumerable<OrganizerResponse>> ExecuteAsync()
    {
        var organizers = await organizerRepository.GetAllAsync();
        return organizers.Select(o => new OrganizerResponse(o.Id, o.Name, o.Email, o.Phone, o.Document, o.LogoUrl, o.Description, o.IsActive, o.CreatedAt, o.UpdatedAt));
    }
}
