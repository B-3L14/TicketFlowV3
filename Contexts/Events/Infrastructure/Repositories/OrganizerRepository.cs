using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Infrastructure.Data;

namespace TicketFlow.Contexts.Events.Infrastructure.Repositories;

public class OrganizerRepository(EventsDbContext context) : IOrganizerRepository
{
    public async Task<Organizer?> GetByIdAsync(Guid id) =>
        await context.Organizers.FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Organizer>> GetAllAsync() =>
        await context.Organizers
            .Where(o => o.IsActive)
            .OrderBy(o => o.Name)
            .ToListAsync();

    public async Task AddAsync(Organizer organizer)
    {
        await context.Organizers.AddAsync(organizer);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Organizer organizer)
    {
        context.Organizers.Update(organizer);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id) =>
        await context.Organizers.AnyAsync(o => o.Id == id);
}
