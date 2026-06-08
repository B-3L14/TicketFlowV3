using TicketFlow.Contexts.Auth.Application.DTOs;
using TicketFlow.Contexts.Auth.Domain.Entities;
using TicketFlow.Contexts.Auth.Domain.Ports;
using TicketFlow.Contexts.Auth.Domain.ValueObjects;
using TicketFlow.Contexts.Auth.Domain.Events; // <-- Novo Import
using TicketFlow.Shared.Domain; // <-- Novo Import

namespace TicketFlow.Contexts.Auth.Application.UseCases;

public class RegisterUseCase(
    IUserRepository userRepository,
    ITokenService tokenService,
    IPasswordHasher passwordHasher,
    IDomainEventDispatcher eventDispatcher) // <-- Injetando o Dispatcher
{
    public async Task<RegisterResponse> ExecuteAsync(RegisterRequest request)
    {
        Email emailVo = new Email(request.Email);
        Cpf cpfVo = new Cpf(request.Cpf);

        var emailInUse = await userRepository.ExistsByEmailAsync(emailVo);
        if (emailInUse)
            throw new InvalidOperationException("Este e-mail já está em uso.");

        var passwordHashString = passwordHasher.Hash(request.Password);

        var user = User.Create(request.Name, emailVo, cpfVo, passwordHashString, request.Role);

        await userRepository.AddAsync(user);

        // 🚀 Dispara o Evento de Domínio de forma assíncrona
        await eventDispatcher.DispatchAsync(new UserRegisteredEvent(
            user.Id, 
            user.Name, 
            user.Email.Valor, 
            user.Role.ToString()
        ));

        var token = tokenService.GenerateToken(user);
        return new RegisterResponse(token, user.Name, user.Role.ToString());
    }
}