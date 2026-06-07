using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Sales.Domain.Entities;
using TicketFlow.Contexts.Sales.Domain.Ports;
using TicketFlow.Contexts.Sales.Infrastructure.Data;

namespace TicketFlow.Contexts.Sales.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly SalesDbContext _context;

        public ReservationRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync(TemporaryReservation reserva)
        {
            await _context.Reservas.AddAsync(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task<TemporaryReservation?> ObterPorIdAsync(Guid id)
        {
            return await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task RemoverAsync(Guid id)
        {
            var reserva = await ObterPorIdAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoverExpiradas()
        {
            var agora = DateTime.UtcNow;

            // Transforma a lógica da propriedade Ativa em uma query SQL baseada no VO
            // "DELETE FROM Reservas WHERE Validade_ExpiraEm <= @agora"
            await _context.Reservas
                .Where(r => r.Validade.ExpiraEm <= agora)
                .ExecuteDeleteAsync();
        }
    }
}