using TicketFlow.Contexts.Auth.Application.DTOs;
using TicketFlow.Contexts.Auth.Domain.Entities;
using TicketFlow.Contexts.Auth.Domain.Ports;
using TicketFlow.Contexts.Auth.Domain.ValueObjects; // <-- Importar os VOs

namespace TicketFlow.Contexts.Auth.Application.UseCases;

public class RegisterUseCase(
    IUserRepository userRepository,
    ITokenService tokenService,
    IPasswordHasher passwordHasher)
{
    public async Task<RegisterResponse> ExecuteAsync(RegisterRequest request)
    {
        // 1. Fail-Fast: Instancia os VOs antes de qualquer coisa.
        // Se o e-mail ou CPF forem inválidos, lança a exceção AQUI.
        Email emailVo = new Email(request.Email);
        Cpf cpfVo = new Cpf(request.Cpf);

        // 2. Consulta no banco já enviando o Objeto válido
        var emailInUse = await userRepository.ExistsByEmailAsync(emailVo);
        if (emailInUse)
            throw new InvalidOperationException("Este e-mail já está em uso.");

        var passwordHashString = passwordHasher.Hash(request.Password);

        // 3. Cria a entidade passando os VOs instanciados
        var user = User.Create(request.Name, emailVo, cpfVo, passwordHashString, request.Role);

        await userRepository.AddAsync(user);

        var token = tokenService.GenerateToken(user);
        return new RegisterResponse(token, user.Name, user.Role.ToString());
    }
}