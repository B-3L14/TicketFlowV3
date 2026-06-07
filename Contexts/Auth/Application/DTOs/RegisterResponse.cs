namespace TicketFlow.Contexts.Auth.Application.DTOs
{
    public record RegisterResponse(
        string Token, 
        string Name, 
        string Role
    );
}
