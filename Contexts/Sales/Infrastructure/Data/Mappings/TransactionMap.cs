using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.ValueObjects;

namespace TicketFlow.Contexts.Sales.Infrastructure.Data.Mappings
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            // Chave Primária
            builder.HasKey(t => t.Id);

            // Mapeamento do VO Dinheiro
            builder.Property(t => t.Valor)
                .HasConversion(
                    vo => vo.Valor,
                    dbValue => new Dinheiro(dbValue))
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Mapeamento do VO TokenPagamento
            builder.Property(t => t.TokenPagamento)
                .HasConversion(
                    vo => vo.Valor, 
                    dbValue => new TokenPagamento(dbValue))
                .HasColumnType("varchar(255)") // É uma boa prática limitar o tamanho do token no banco
                .IsRequired();

            // Mapeamento do Enum Status
            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>(); // Opcional, mas muito recomendado: Salva o Enum como texto ("Pendente", "Aprovado") em vez de 0, 1, 2. Facilita muito a leitura direta no banco!

            // Propriedades Básicas
            builder.Property(t => t.PedidoId)
                .IsRequired();

            builder.Property(t => t.CriadoEm)
                .IsRequired();

            builder.Property(t => t.ProcessadoEm)
                .IsRequired(false); // ProcessadoEm é Nullable (DateTime?)

            // 🚀 Otimização de Performance (Índices)
            // Como é muito comum buscar transações a partir de um Pedido,
            // criar um índice para o PedidoId deixa a query muito mais rápida.
            builder.HasIndex(t => t.PedidoId)
                .HasDatabaseName("IX_Transactions_PedidoId");
        }
    }
}