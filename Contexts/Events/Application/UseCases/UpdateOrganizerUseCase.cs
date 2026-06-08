using TicketFlow.Contexts.Auth.Domain.ValueObjects;
using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class UpdateOrganizerUseCase(IOrganizerRepository organizerRepository)
{
    public async Task<OrganizerResponse> ExecuteAsync(Guid id, UpdateOrganizerRequest request)
    {
        var organizer = await organizerRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Organizador '{id}' não encontrado.");

        Email emailVo = new Email(request.Email);
        organizer.Update(request.Name, emailVo, request.Phone, request.Document, request.LogoUrl, request.Description);
        await organizerRepository.UpdateAsync(organizer);
        return new OrganizerResponse(organizer.Id, organizer.Name, organizer.Email, organizer.Phone, organizer.Document, organizer.LogoUrl, organizer.Description, organizer.IsActive, organizer.CreatedAt, organizer.UpdatedAt);
    }
}
