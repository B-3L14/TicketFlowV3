using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Sales.Domain.Ports;

namespace TicketFlow.Contexts.Sales.Infrastructure.Adapters
{
    public class EventCatalogAdapter : IEventCatalogService
    {
        private readonly IEventRepository _eventRepository;

        public EventCatalogAdapter(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<decimal> GetPriceAsync(Guid eventoId)
        {
            // Vai ao contexto de Eventos buscar o evento com os seus lotes
            var @event = await _eventRepository.GetByIdWithDetailsAsync(eventoId)
                ?? throw new KeyNotFoundException($"Evento '{eventoId}' não encontrado no catálogo.");

            // Retorna o preço atual (que já calcula se há lotes ativos ou usa o preço base)
            return @event.CurrentPrice.Amount;
        }
    }
}