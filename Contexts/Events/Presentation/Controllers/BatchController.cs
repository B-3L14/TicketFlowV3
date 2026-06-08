using Microsoft.AspNetCore.Mvc;
using TicketFlow.Contexts.Events.Application.DTOs;
using TicketFlow.Contexts.Events.Application.UseCases;

namespace TicketFlow.Contexts.Events.Presentation.Controllers;

[ApiController]
[Route("api/v1/events/{eventId:guid}/batches")]
public class BatchController(
    CreateBatchUseCase createBatchUseCase,
    UpdateBatchUseCase updateBatchUseCase,
    GetBatchesByEventUseCase getBatchesByEventUseCase,
    DeleteBatchUseCase deleteBatchUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByEvent(Guid eventId)
    {
        try
        {
            var result = await getBatchesByEventUseCase.ExecuteAsync(eventId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Erro = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid eventId, [FromBody] CreateBatchRequest request)
    {
        try
        {
            var requestWithId = request with { EventId = eventId };
            var result = await createBatchUseCase.ExecuteAsync(requestWithId);
            return Created(string.Empty, result);
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

    [HttpPut("{batchId:guid}")]
    public async Task<IActionResult> Update(Guid eventId, Guid batchId, [FromBody] UpdateBatchRequest request)
    {
        try
        {
            var result = await updateBatchUseCase.ExecuteAsync(batchId, request);
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

    [HttpDelete("{batchId:guid}")]
    public async Task<IActionResult> Delete(Guid eventId, Guid batchId)
    {
        try
        {
            await deleteBatchUseCase.ExecuteAsync(eventId, batchId);
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
