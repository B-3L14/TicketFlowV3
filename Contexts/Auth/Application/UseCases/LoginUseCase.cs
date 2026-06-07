using TicketFlow.Contexts.Auth.Application.DTOs;
using TicketFlow.Contexts.Auth.Domain.Ports;
using TicketFlow.Contexts.Auth.Domain.ValueObjects; // <-- Importar o VO

namespace TicketFlow.Contexts.Auth.Application.UseCases;

public class LoginUseCase(
    IUserRepository userRepository,
    ITokenService tokenService,
    IPasswordHasher passwordHasher)
{
    public async Task<LoginResponse> ExecuteAsync(LoginRequest request)
    {
        // Impede que queries de banco ocorram se a formatação for obviamente falha
        Email emailVo = new Email(request.Email);

        var user = await userRepository.GetByEmailAsync(emailVo);
        if (user is null)
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        var passwordValid = passwordHasher.Verify(request.Password, user.PasswordHash.Valor);
        if (!passwordValid)
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        var token = tokenService.GenerateToken(user);
        return new LoginResponse(token, user.Name, user.Role.ToString());
    }
}