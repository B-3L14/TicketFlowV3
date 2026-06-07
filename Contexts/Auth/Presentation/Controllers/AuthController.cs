using Microsoft.AspNetCore.Mvc;
using TicketFlow.Contexts.Auth.Application.DTOs;
using TicketFlow.Contexts.Auth.Application.UseCases;

namespace TicketFlow.Contexts.Auth.Presentation.Controllers;

[ApiController]
[Route("api/v1/auth")] // Opcional: Adicionado api/v1 para manter consistência com o contexto de Sales
public class AuthController(RegisterUseCase registerUseCase, LoginUseCase loginUseCase) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await registerUseCase.ExecuteAsync(request);
            return Created(string.Empty, response);
        }
        catch (ArgumentException ex)
        {
            // Captura erros dos Value Objects (ex: CPF inválido, Email em formato incorreto)
            return BadRequest(new { Erro = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Captura erros de regra de negócio (ex: E-mail já em uso)
            return Conflict(new { Erro = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await loginUseCase.ExecuteAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            // Captura erros de credenciais inválidas
            return Unauthorized(new { Erro = ex.Message });
        }
        catch (ArgumentException ex)
        {
            // Prevenção caso passemos a validar o formato do e-mail no login futuramente
            return BadRequest(new { Erro = ex.Message });
        }
    }
}