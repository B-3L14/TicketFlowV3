using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.ValueObjects;

namespace TicketFlow.Contexts.Sales.Infrastructure.Data.Mappings
{
    public class TicketMap : IEntityTypeConfiguration<Ingresso>
    {
        public void Configure(EntityTypeBuilder<Ingresso> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(i => i.Id);

            // Mapeando o HashIngresso (Value Object) para uma string no banco
            builder.Property(i => i.Hash)
                .HasConversion(
                    vo => vo.Valor,
                    dbValue => new HashIngresso(dbValue))
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}