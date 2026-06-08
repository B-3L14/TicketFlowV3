using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Events.Domain.Entities;

namespace TicketFlow.Contexts.Events.Infrastructure.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnType("varchar(500)");

        builder.Property(c => c.IconUrl)
            .HasColumnType("varchar(500)");

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);
    }
}
