using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Infrastructure.Data;

namespace TicketFlow.Contexts.Events.Infrastructure.Repositories;

public class CategoryRepository(EventsDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id) =>
        await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Category>> GetAllAsync() =>
        await context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();

    public async Task AddAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id) =>
        await context.Categories.AnyAsync(c => c.Id == id);
}
