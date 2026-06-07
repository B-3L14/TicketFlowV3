using Microsoft.EntityFrameworkCore;
using TicketFlow.Contexts.Auth.Domain.Entities;
using TicketFlow.Contexts.Auth.Domain.Ports;
using TicketFlow.Contexts.Auth.Domain.ValueObjects;
using TicketFlow.Contexts.Auth.Infrastructure.Data;

namespace TicketFlow.Contexts.Auth.Infrastructure.Repositories;

public class UserRepository(AuthDbContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(Email email)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        return await context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}