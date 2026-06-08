using TicketFlow.Contexts.Auth.Domain.ValueObjects;
using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class CreateOrganizerUseCase(IOrganizerRepository organizerRepository)
{
    public async Task<OrganizerResponse> ExecuteAsync(CreateOrganizerRequest request)
    {
        Email emailVo = new Email(request.Email);

        var organizer = Organizer.Create(request.Name, emailVo, request.Phone, request.Document);

        if (request.LogoUrl is not null || request.Description is not null)
            organizer.Update(request.Name, emailVo, request.Phone, request.Document, request.LogoUrl, request.Description);

        await organizerRepository.AddAsync(organizer);
        return new OrganizerResponse(organizer.Id, organizer.Name, organizer.Email, organizer.Phone, organizer.Document, organizer.LogoUrl, organizer.Description, organizer.IsActive, organizer.CreatedAt, organizer.UpdatedAt);
    }
}
