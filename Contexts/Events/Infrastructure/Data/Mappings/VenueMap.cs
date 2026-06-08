using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.ValueObjects;

namespace TicketFlow.Contexts.Events.Infrastructure.Data.Mappings;

public class VenueMap : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("Venues");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder.Property(v => v.Capacity)
            .IsRequired();

        // Conversão do VO Address (colunas achatadas na tabela)
        builder.Property(v => v.Address)
            .HasConversion(
                vo => $"{vo.Street}|{vo.Number}|{vo.Complement}|{vo.Neighborhood}|{vo.City}|{vo.State}|{vo.ZipCode}|{vo.Country}",
                dbValue => ParseAddress(dbValue))
            .HasColumnType("varchar(800)")
            .IsRequired()
            .HasColumnName("Address");

        builder.Property(v => v.MapUrl)
            .HasColumnType("varchar(500)");

        builder.Property(v => v.Description)
            .HasColumnType("varchar(1000)");

        builder.Property(v => v.IsActive)
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .IsRequired();

        builder.Property(v => v.UpdatedAt);
    }

    private static Address ParseAddress(string value)
    {
        var parts = value.Split('|');
        return new Address(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6], parts[7]);
    }
}
