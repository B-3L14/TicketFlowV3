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

            // 4️⃣ ESSENCIAL: Evita o mesmo erro quando os ingressos forem gerados após o pagamento.
            builder.Property(i => i.Id).ValueGeneratedNever();

            builder.Property(i => i.Hash)
                .HasConversion(
                    vo => vo.Valor,
                    dbValue => new HashIngresso(dbValue))
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}