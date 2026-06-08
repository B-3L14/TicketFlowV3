using TicketFlow.Shared.Domain;

namespace TicketFlow.Contexts.Auth.Domain.Events;

public record UserRegisteredEvent(
    Guid UserId,
    string Name,
    string Email,
    string Role
) : IDomainEvent;