using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Infrastructure.Data.Mappings;

public class EventMap : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(e => e.EventType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Description)
            .HasColumnType("varchar(2000)")
            .IsRequired();

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.Property(e => e.Capacity)
            .IsRequired();

        // Conversão do VO TicketPrice (persiste apenas o Amount; Currency sempre BRL)
        builder.Property(e => e.BasePrice)
            .HasConversion(
                vo => vo.Amount,
                dbValue => new TicketPrice(dbValue))
            .HasColumnType("decimal(10,2)")
            .IsRequired()
            .HasColumnName("BasePrice");

        builder.Property(e => e.ImageUrl)
            .HasColumnType("varchar(500)");

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt);

        // Relacionamentos
        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events)
            .HasForeignKey(e => e.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Organizer)
            .WithMany(o => o.Events)
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Batches)
            .WithOne()
            .HasForeignKey(b => b.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
