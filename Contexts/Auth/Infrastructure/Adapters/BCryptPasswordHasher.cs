using TicketFlow.Contexts.Auth.Domain.Ports;
using BCrypt.Net;

namespace TicketFlow.Contexts.Auth.Infrastructure.Adapters
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}