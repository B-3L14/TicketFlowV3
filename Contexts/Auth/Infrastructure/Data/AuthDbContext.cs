using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Auth.Domain.Entities;

namespace TicketFlow.Contexts.Auth.Infrastructure.Data
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Filtra para aplicar APENAS as configurações (Maps) que estão no namespace do Auth
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AuthDbContext).Assembly,
                type => type.Namespace != null && type.Namespace.Contains("Contexts.Auth"));

            base.OnModelCreating(modelBuilder);
        }
    }
}