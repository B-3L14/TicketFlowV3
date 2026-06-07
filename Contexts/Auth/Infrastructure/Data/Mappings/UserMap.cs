using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketFlow.Contexts.Auth.Domain.Entities;
using TicketFlow.Contexts.Auth.Domain.ValueObjects; // <-- Importante: Importar os VOs

namespace TicketFlow.Contexts.Auth.Infrastructure.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // Chave Primária
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasColumnType("varchar(150)")
                .IsRequired();

            // 👇 Conversão do VO Email
            builder.Property(u => u.Email)
                .HasConversion(
                    vo => vo.Valor,
                    dbValue => new Email(dbValue))
                .HasColumnType("varchar(150)")
                .IsRequired();

            // 👇 Conversão do VO Cpf
            builder.Property(u => u.Cpf)
                .HasConversion(
                    vo => vo.Valor,
                    dbValue => new Cpf(dbValue))
                .HasColumnType("varchar(11)")
                .IsRequired();

            // 👇 Conversão do VO PasswordHash
            builder.Property(u => u.PasswordHash)
                .HasConversion(
                    vo => vo.Valor,
                    dbValue => new PasswordHash(dbValue))
                .HasColumnType("varchar(255)")
                .IsRequired();

            // Enum Role (convertido para string no banco)
            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>();

            // ÍNDICES ÚNICOS
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            builder.HasIndex(u => u.Cpf)
                .IsUnique()
                .HasDatabaseName("IX_Users_Cpf");
        }
    }
}