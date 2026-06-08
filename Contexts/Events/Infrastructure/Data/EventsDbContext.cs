using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Events.Domain.Entities;

namespace TicketFlow.Contexts.Events.Infrastructure.Data;

public class EventsDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Batch> Batches { get; set; }
    public DbSet<Organizer> Organizers { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Category> Categories { get; set; }

    public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Filtra para aplicar APENAS as configurações (Maps) que estão no namespace do Events
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(EventsDbContext).Assembly,
            type => type.Namespace != null && type.Namespace.Contains("Contexts.Events"));

        base.OnModelCreating(modelBuilder);
    }
}
