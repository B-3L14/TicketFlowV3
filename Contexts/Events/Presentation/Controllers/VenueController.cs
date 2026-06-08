using Microsoft.AspNetCore.Mvc;
using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Application.UseCases;

namespace TicketFlow.Contexts.Events.Presentation.Controllers;

[ApiController]
[Route("api/v1/venues")]
public class VenueController(
    CreateVenueUseCase createVenueUseCase,
    UpdateVenueUseCase updateVenueUseCase,
    GetVenueUseCase getVenueUseCase,
    ListVenuesUseCase listVenuesUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await listVenuesUseCase.ExecuteAsync();
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
            var result = await getVenueUseCase.ExecuteAsync(id);
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
    public async Task<IActionResult> Create([FromBody] CreateVenueRequest request)
    {
        try
        {
            var result = await createVenueUseCase.ExecuteAsync(request);
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
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVenueRequest request)
    {
        try
        {
            var result = await updateVenueUseCase.ExecuteAsync(id, request);
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
