using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class GetOrganizerUseCase(IOrganizerRepository organizerRepository)
{
    public async Task<OrganizerResponse> ExecuteAsync(Guid id)
    {
        var organizer = await organizerRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Organizador '{id}' não encontrado.");

        return new OrganizerResponse(organizer.Id, organizer.Name, organizer.Email, organizer.Phone, organizer.Document, organizer.LogoUrl, organizer.Description, organizer.IsActive, organizer.CreatedAt, organizer.UpdatedAt);
    }
}
