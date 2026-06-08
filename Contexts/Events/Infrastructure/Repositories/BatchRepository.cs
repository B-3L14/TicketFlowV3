using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Infrastructure.Data;

namespace TicketFlow.Contexts.Events.Infrastructure.Repositories;

public class BatchRepository(EventsDbContext context) : IBatchRepository
{
    public async Task<Batch?> GetByIdAsync(Guid id) =>
        await context.Batches.FirstOrDefaultAsync(b => b.Id == id);

    public async Task<IEnumerable<Batch>> GetByEventAsync(Guid eventId) =>
        await context.Batches
            .Where(b => b.EventId == eventId)
            .OrderBy(b => b.Order)
            .ToListAsync();

    public async Task AddAsync(Batch batch)
    {
        await context.Batches.AddAsync(batch);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Batch batch)
    {
        context.Batches.Update(batch);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var batch = await GetByIdAsync(id);
        if (batch is not null)
        {
            context.Batches.Remove(batch);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id) =>
        await context.Batches.AnyAsync(b => b.Id == id);
}
