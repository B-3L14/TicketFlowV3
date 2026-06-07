namespace TicketFlow.Contexts.Auth.Application.DTOs
{
    public record LoginResponse(
        string Token, 
        string Name, 
        string Role
    ); 
}
