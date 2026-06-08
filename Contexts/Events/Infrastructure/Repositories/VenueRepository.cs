using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Contexts.Events.Infrastructure.Data;

namespace TicketFlow.Contexts.Events.Infrastructure.Repositories;

public class VenueRepository(EventsDbContext context) : IVenueRepository
{
    public async Task<Venue?> GetByIdAsync(Guid id) =>
        await context.Venues.FirstOrDefaultAsync(v => v.Id == id);

    public async Task<IEnumerable<Venue>> GetAllAsync() =>
        await context.Venues
            .Where(v => v.IsActive)
            .OrderBy(v => v.Name)
            .ToListAsync();

    public async Task AddAsync(Venue venue)
    {
        await context.Venues.AddAsync(venue);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Venue venue)
    {
        context.Venues.Update(venue);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id) =>
        await context.Venues.AnyAsync(v => v.Id == id);
}
