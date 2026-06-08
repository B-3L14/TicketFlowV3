using Microsoft.AspNetCore.Mvc;
using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Application.UseCases;

namespace TicketFlow.Contexts.Events.Presentation.Controllers;

[ApiController]
[Route("api/v1/events")]
public class EventController(
    CreateEventUseCase createEventUseCase,
    UpdateEventUseCase updateEventUseCase,
    GetEventUseCase getEventUseCase,
    ListEventsUseCase listEventsUseCase,
    ListUpcomingEventsUseCase listUpcomingEventsUseCase,
    PublishEventUseCase publishEventUseCase,
    CancelEventUseCase cancelEventUseCase,
    DeleteEventUseCase deleteEventUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await listEventsUseCase.ExecuteAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var result = await listUpcomingEventsUseCase.ExecuteAsync(page, pageSize);
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
            var result = await getEventUseCase.ExecuteAsync(id);
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
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        try
        {
            var result = await createEventUseCase.ExecuteAsync(request);
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
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventRequest request)
    {
        try
        {
            var result = await updateEventUseCase.ExecuteAsync(id, request);
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

    [HttpPatch("{id:guid}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        try
        {
            var result = await publishEventUseCase.ExecuteAsync(id);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            var result = await cancelEventUseCase.ExecuteAsync(id);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await deleteEventUseCase.ExecuteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }
}
