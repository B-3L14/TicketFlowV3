using TicketFlow.Contexts.Events.Domain.Entities;

namespace TicketFlow.Contexts.Events.Domain.Ports;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task<bool> ExistsAsync(Guid id);
}
