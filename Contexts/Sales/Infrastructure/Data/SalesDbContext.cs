using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Domain.Entities; 

namespace TicketFlow.Contexts.Sales.Infrastructure.Data
{
    public class SalesDbContext : DbContext
    {
        public DbSet<Order> Pedidos { get; set; }
        public DbSet<Transaction> Transacoes { get; set; }
        public DbSet<TemporaryReservation> Reservas { get; set; }
        public DbSet<Ingresso> Ingressos { get; set; }

        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Filtra para aplicar APENAS as configurações (Maps) que estão no namespace do Sales
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SalesDbContext).Assembly,
                type => type.Namespace != null && type.Namespace.Contains("Contexts.Sales"));

            base.OnModelCreating(modelBuilder);
        }
    }
}