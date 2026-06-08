using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Infrastructure.Data.Mappings;

public class BatchMap : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("Batches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.EventId)
            .IsRequired();

        builder.Property(b => b.Name)
            .HasColumnType("varchar(100)")
            .IsRequired();

        // Conversão do VO TicketPrice (persiste apenas o Amount; Currency sempre BRL)
        builder.Property(b => b.Price)
            .HasConversion(
                vo => vo.Amount,
                dbValue => new TicketPrice(dbValue))
            .HasColumnType("decimal(10,2)")
            .IsRequired()
            .HasColumnName("Price");

        builder.Property(b => b.TotalTickets)
            .IsRequired();

        builder.Property(b => b.SoldTickets)
            .IsRequired();

        builder.Property(b => b.SaleStartDate)
            .IsRequired();

        builder.Property(b => b.SaleEndDate)
            .IsRequired();

        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(b => b.Order)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        builder.Property(b => b.UpdatedAt);
    }
}
