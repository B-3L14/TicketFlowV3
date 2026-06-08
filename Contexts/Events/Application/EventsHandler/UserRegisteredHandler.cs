using TicketFlow.Contexts.Auth.Domain.Events;
using TicketFlow.Contexts.Auth.Domain.ValueObjects;
using TicketFlow.Contexts.Events.Domain.Entities;
using TicketFlow.Contexts.Events.Domain.Ports;
using TicketFlow.Shared.Application;

namespace TicketFlow.Contexts.Events.Application.EventHandlers;

public class UserRegisteredHandler(IOrganizerRepository organizerRepository)
    : IEventHandler<UserRegisteredEvent>
{
    public async Task HandleAsync(UserRegisteredEvent @event)
    {
        // Regra de Negócio: Só cria um organizador se o usuário registrado possuir o papel de 'Manager'
        if (@event.Role == "Manager")
        {
            // Instancia o Value Object de e-mail do escopo local
            Email emailVo = new Email(@event.Email);

            // Cria o Organizer reaproveitando o mesmo Id original gerado na tabela de Usuários
            var organizer = Organizer.CreateWithId(
                @event.UserId,
                @event.Name,
                emailVo,
                "0000000000" // Telefone temporário padrão, o usuário poderá editar no perfil depois
            );

            await organizerRepository.AddAsync(organizer);
        }
    }
}