using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class CreateEventUseCase(
    IEventRepository eventRepository,
    IVenueRepository venueRepository,
    IOrganizerRepository organizerRepository,
    ICategoryRepository categoryRepository)
{
    public async Task<EventResponse> ExecuteAsync(CreateEventRequest request)
    {
        if (!await venueRepository.ExistsAsync(request.VenueId))
            throw new ArgumentException($"Local '{request.VenueId}' não encontrado.");
        if (!await organizerRepository.ExistsAsync(request.OrganizerId))
            throw new ArgumentException($"Organizador '{request.OrganizerId}' não encontrado.");
        if (!await categoryRepository.ExistsAsync(request.CategoryId))
            throw new ArgumentException($"Categoria '{request.CategoryId}' não encontrada.");

        var price = new TicketPrice(request.BasePrice);
        var @event = Event.Create(
            request.Name,
            request.EventType,
            request.Description,
            request.Date,
            request.StartTime,
            request.Capacity,
            price,
            request.VenueId,
            request.OrganizerId,
            request.CategoryId,
            request.ImageUrl);

        await eventRepository.AddAsync(@event);

        var created = await eventRepository.GetByIdWithDetailsAsync(@event.Id)
            ?? throw new InvalidOperationException("Erro ao recuperar evento criado.");

        return EventProjection.ToResponse(created);
    }
}
