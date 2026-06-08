using TicketFlow.Contexts.Events.Domain.Entities;

namespace TicketFlow.Contexts.Events.Domain.Ports;

public interface IOrganizerRepository
{
    Task<Organizer?> GetByIdAsync(Guid id);
    Task<IEnumerable<Organizer>> GetAllAsync();
    Task AddAsync(Organizer organizer);
    Task UpdateAsync(Organizer organizer);
    Task<bool> ExistsAsync(Guid id);
}
