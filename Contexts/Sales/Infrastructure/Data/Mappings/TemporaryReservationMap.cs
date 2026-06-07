using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.ValueObjects;

namespace TicketFlow.Contexts.Sales.Infrastructure.Data.Mappings
{
    public class TemporaryReservationMap : IEntityTypeConfiguration<TemporaryReservation>
    {
        public void Configure(EntityTypeBuilder<TemporaryReservation> builder)
        {
            builder.ToTable("TemporaryReservations");

            // Chave Primária
            builder.HasKey(r => r.Id);

            // Propriedades Básicas
            builder.Property(r => r.EventoId)
                .IsRequired();

            builder.Property(r => r.ClienteId)
                .IsRequired();

            // ── MAPEAMENTO VO: QuantidadeIngressos (HasConversion) ──────────
            // Como é um valor único, convertemos direto para uma coluna INT
            builder.Property(r => r.Quantidade)
                .HasConversion(
                    vo => vo.Valor, // Troque 'Valor' pela propriedade correta dentro do seu record, se for diferente
                    dbValue => new QuantidadeIngressos(dbValue))
                .HasColumnType("int")
                .IsRequired();

            // ── MAPEAMENTO VO: PeriodoValidade (OwnsOne) ────────────────────
            // Como tem duas propriedades, abrimos em duas colunas na mesma tabela
            builder.OwnsOne(r => r.Validade, validade =>
            {
                validade.Property(v => v.CriadaEm)
                        .HasColumnName("ValidadeCriadaEm")
                        .IsRequired();

                validade.Property(v => v.ExpiraEm)
                        .HasColumnName("ValidadeExpiraEm")
                        .IsRequired();
            });

            // ── IGNORAR PROPRIEDADES COMPUTADAS ─────────────────────────────
            // Avisa ao EF Core: "Estas propriedades são apenas lógica de negócio no C#. 
            // Não tente criar colunas no banco de dados para elas!"
            builder.Ignore(r => r.Ativa);
            builder.Ignore(r => r.TempoRestante);

            // ── ÍNDICES PARA PERFORMANCE ────────────────────────────────────
            // É muito provável que você precise buscar reservas por Evento ou por Cliente
            builder.HasIndex(r => r.EventoId)
                .HasDatabaseName("IX_Reservations_EventoId");

            builder.HasIndex(r => r.ClienteId)
                .HasDatabaseName("IX_Reservations_ClienteId");
        }
    }
}