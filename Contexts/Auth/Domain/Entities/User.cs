using TicketFlow.Contexts.Auth.Domain.Enums;
using TicketFlow.Contexts.Auth.Domain.ValueObjects;

namespace TicketFlow.Contexts.Auth.Domain.Entities;

public class User
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public Email Email { get; private set; } // Usando o VO
    public Cpf Cpf { get; private set; }     // Usando o VO
    public Roles Role { get; private set; }
    public PasswordHash PasswordHash { get; private set; } // Usando o VO

    // Construtor vazio para o Entity Framework
    private User() { }

    internal User(string name, Email email, Cpf cpf, PasswordHash passwordHash, Roles role)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do usuário é obrigatório.");

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Cpf = cpf;
        Role = role;
        PasswordHash = passwordHash;
    }

    public static User Create(string name, Email email, Cpf cpf, PasswordHash passwordHash, Roles role)
    {
        // A conversão implícita dos VOs fará a validação automaticamente ao passar as strings
        return new User(name, email, cpf, passwordHash, role);
    }
}