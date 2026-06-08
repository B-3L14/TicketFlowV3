using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.ValueObjects;

namespace TicketFlow.Contexts.Sales.Infrastructure.Data.Mappings
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            // 1️⃣ ESSENCIAL: Diz ao EF que nós geramos o ID no C#, não no banco de dados.
            builder.Property(o => o.Id).ValueGeneratedNever();

            var navigationItens = builder.Metadata.FindNavigation(nameof(Order.Itens));
            navigationItens?.SetPropertyAccessMode(PropertyAccessMode.Field);

            var navigationIngressos = builder.Metadata.FindNavigation(nameof(Order.Ingressos));
            navigationIngressos?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // 2️⃣ Mapeamento da relação Pedido -> Ingressos (para criar a Foreign Key)
            builder.HasMany(o => o.Ingressos)
                   .WithOne()
                   .HasForeignKey("OrderId") // Cria uma coluna invisível 'OrderId' na tabela Tickets
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsMany(o => o.Itens, item =>
            {
                item.ToTable("OrderItems");
                item.HasKey(i => i.Id);

                // 3️⃣ ESSENCIAL: Evita o erro de Concorrência ao adicionar novos itens!
                item.Property(i => i.Id).ValueGeneratedNever();

                item.WithOwner().HasForeignKey("OrderId");

                item.Property(i => i.PrecoUnitario)
                    .HasConversion(
                        vo => vo.Valor,
                        dbValue => new Dinheiro(dbValue))
                    .HasColumnType("decimal(18,2)");

                item.Property(i => i.Quantidade)
                    .HasConversion(
                        vo => vo.Valor,
                        dbValue => new QuantidadeIngressos(dbValue));
            });
        }
    }
}