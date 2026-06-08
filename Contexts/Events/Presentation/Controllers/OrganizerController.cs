using Microsoft.AspNetCore.Mvc;
using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Application.UseCases;

namespace TicketFlow.Contexts.Events.Presentation.Controllers;

[ApiController]
[Route("api/v1/organizers")]
public class OrganizerController(
    CreateOrganizerUseCase createOrganizerUseCase,
    UpdateOrganizerUseCase updateOrganizerUseCase,
    GetOrganizerUseCase getOrganizerUseCase,
    ListOrganizersUseCase listOrganizersUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await listOrganizersUseCase.ExecuteAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await getOrganizerUseCase.ExecuteAsync(id);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizerRequest request)
    {
        try
        {
            var result = await createOrganizerUseCase.ExecuteAsync(request);
            return Created(string.Empty, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizerRequest request)
    {
        try
        {
            var result = await updateOrganizerUseCase.ExecuteAsync(id, request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Erro = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }
}
