using TicketFlow.Contexts.Events.Domain.Enums;
using TicketFlow.Contexts.Events.Domain.Ports;

namespace TicketFlow.Contexts.Events.Application.UseCases;

public class DeleteEventUseCase(IEventRepository eventRepository)
{
    public async Task ExecuteAsync(Guid id)
    {
        var @event = await eventRepository.GetByIdWithDetailsAsync(id) // Alterado para buscar com detalhes (lotes)
            ?? throw new InvalidOperationException($"Evento '{id}' não encontrado.");

        if (@event.Status == EventStatus.Published)
            throw new InvalidOperationException("Não é possível excluir um evento publicado. Cancele-o primeiro.");

        // NOVA REGRA: Impede a exclusão se houver ingressos vendidos
        if (@event.TotalSold > 0)
            throw new InvalidOperationException("Não é possível excluir um evento que já possui ingressos vendidos. Utilize a opção de Cancelar.");

        await eventRepository.DeleteAsync(id);
    }
}