using TicketFlow.Contexts.Auth.Domain.Entities;

namespace TicketFlow.Contexts.Auth.Domain.Ports
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}