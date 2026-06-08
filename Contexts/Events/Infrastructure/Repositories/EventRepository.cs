using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Enums;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Infrastructure.Data;

namespace TicketFlow.Contexts.Events.Infrastructure.Repositories;

public class EventRepository(EventsDbContext context) : IEventRepository
{
    public async Task<Event?> GetByIdAsync(Guid id) =>
        await context.Events.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Event?> GetByIdWithDetailsAsync(Guid id) =>
        await context.Events
            .Include(e => e.Venue)
            .Include(e => e.Organizer)
            .Include(e => e.Category)
            .Include(e => e.Batches)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Event>> GetAllAsync() =>
        await context.Events
            .Include(e => e.Venue)
            .Include(e => e.Organizer)
            .Include(e => e.Category)
            .OrderByDescending(e => e.Date)
            .ToListAsync();

    public async Task<IEnumerable<Event>> GetByOrganizerAsync(Guid organizerId) =>
        await context.Events
            .Include(e => e.Venue)
            .Include(e => e.Category)
            .Where(e => e.OrganizerId == organizerId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();

    public async Task<IEnumerable<Event>> GetByCategoryAsync(Guid categoryId) =>
        await context.Events
            .Include(e => e.Venue)
            .Include(e => e.Organizer)
            .Where(e => e.CategoryId == categoryId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();

    public async Task<IEnumerable<Event>> GetByStatusAsync(EventStatus status) =>
        await context.Events
            .Include(e => e.Venue)
            .Include(e => e.Organizer)
            .Include(e => e.Category)
            .Where(e => e.Status == status)
            .OrderBy(e => e.Date)
            .ToListAsync();

    public async Task<IEnumerable<Event>> GetUpcomingAsync(int pageNumber, int pageSize) =>
        await context.Events
            .Include(e => e.Venue)
            .Include(e => e.Organizer)
            .Include(e => e.Category)
            .Include(e => e.Batches)
            .Where(e => e.Status == EventStatus.Published && e.Date > DateTime.UtcNow)
            .OrderBy(e => e.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task AddAsync(Event @event)
    {
        await context.Events.AddAsync(@event);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event @event)
    {
        context.Events.Update(@event);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var @event = await GetByIdAsync(id);
        if (@event is not null)
        {
            context.Events.Remove(@event);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id) =>
        await context.Events.AnyAsync(e => e.Id == id);
}
