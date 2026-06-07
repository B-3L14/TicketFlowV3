using TicketFlow.Contexts.Auth.Domain.Enums;

namespace TicketFlow.Contexts.Auth.Application.DTOs;

public record RegisterRequest(
    string Name,
    string Email,
    string Cpf,
    Roles Role,
    string Password
);