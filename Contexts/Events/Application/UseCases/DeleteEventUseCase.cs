using TicketFlow.Contexts.Events.Domain.Enums;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class DeleteEventUseCase(IEventRepository eventRepository)
{
    public async Task ExecuteAsync(Guid id)
    {
        var @event = await eventRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Evento '{id}' não encontrado.");

        if (@event.Status == EventStatus.Published)
            throw new InvalidOperationException("Não é possível excluir um evento publicado. Cancele-o primeiro.");

        await eventRepository.DeleteAsync(id);
    }
}
