using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Enums;

namespace TicketFlow.Contexts.Events.Domain.Ports;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id);
    Task<Event?> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<Event>> GetAllAsync();
    Task<IEnumerable<Event>> GetByOrganizerAsync(Guid organizerId);
    Task<IEnumerable<Event>> GetByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Event>> GetByStatusAsync(EventStatus status);
    Task<IEnumerable<Event>> GetUpcomingAsync(int pageNumber, int pageSize);
    Task AddAsync(Event @event);
    Task UpdateAsync(Event @event);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
