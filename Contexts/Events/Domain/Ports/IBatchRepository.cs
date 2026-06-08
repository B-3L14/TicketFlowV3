using TicketFlow.Contexts.Events.Domain.Entities;

namespace TicketFlow.Contexts.Events.Domain.Ports;

public interface IBatchRepository
{
    Task<Batch?> GetByIdAsync(Guid id);
    Task<IEnumerable<Batch>> GetByEventAsync(Guid eventId);
    Task AddAsync(Batch batch);
    Task UpdateAsync(Batch batch);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
