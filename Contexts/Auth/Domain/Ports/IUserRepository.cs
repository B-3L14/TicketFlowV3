using TicketFlow.Contexts.Auth.Domain.Entities;
using TicketFlow.Contexts.Auth.Domain.ValueObjects;

namespace TicketFlow.Contexts.Auth.Domain.Ports
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(Email email);
        Task<bool> ExistsByEmailAsync(Email email);
        Task AddAsync(User user);
    }
}
