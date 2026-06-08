using TicketFlow.Contexts.Events.Domain.Entities;

namespace TicketFlow.Contexts.Events.Domain.Ports;

public interface IVenueRepository
{
    Task<Venue?> GetByIdAsync(Guid id);
    Task<IEnumerable<Venue>> GetAllAsync();
    Task AddAsync(Venue venue);
    Task UpdateAsync(Venue venue);
    Task<bool> ExistsAsync(Guid id);
}
