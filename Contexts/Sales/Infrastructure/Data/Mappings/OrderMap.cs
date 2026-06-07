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

            // Mapeia as coleções encapsuladas no domínio
            var navigationItens = builder.Metadata.FindNavigation(nameof(Order.Itens));
            navigationItens?.SetPropertyAccessMode(PropertyAccessMode.Field); // Diz ao EF para usar o _itens backing field

            var navigationIngressos = builder.Metadata.FindNavigation(nameof(Order.Ingressos));
            navigationIngressos?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // Configuração da entidade dependente OrderItem
            builder.OwnsMany(o => o.Itens, item =>
            {
                item.ToTable("OrderItems");
                item.HasKey(i => i.Id);
                item.WithOwner().HasForeignKey("OrderId");

                // Mapeando os Value Objects do item
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