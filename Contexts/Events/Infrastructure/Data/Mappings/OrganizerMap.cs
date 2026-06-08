using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Auth.Domain.ValueObjects;
using TicketFlow.Contexts.Events.Domain.Entities;

namespace TicketFlow.Contexts.Events.Infrastructure.Data.Mappings;

public class OrganizerMap : IEntityTypeConfiguration<Organizer>
{
    public void Configure(EntityTypeBuilder<Organizer> builder)
    {
        builder.ToTable("Organizers");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .HasColumnType("varchar(150)")
            .IsRequired();

        // Conversão do VO Email (compartilhado com o contexto Auth)
        builder.Property(o => o.Email)
            .HasConversion(
                vo => vo.Valor,
                dbValue => new Email(dbValue))
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder.Property(o => o.Phone)
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder.Property(o => o.Document)
            .HasColumnType("varchar(20)");

        builder.Property(o => o.LogoUrl)
            .HasColumnType("varchar(500)");

        builder.Property(o => o.Description)
            .HasColumnType("varchar(1000)");

        builder.Property(o => o.IsActive)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.UpdatedAt);

        builder.HasIndex(o => o.Email)
            .IsUnique()
            .HasDatabaseName("IX_Organizers_Email");
    }
}
