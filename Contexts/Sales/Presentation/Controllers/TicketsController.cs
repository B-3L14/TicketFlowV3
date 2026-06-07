using Microsoft.AspNetCore.Mvc;
using TicketFlow.Contexts.Sales.Application.UseCases;

namespace TicketFlow.Contexts.Sales.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly GetTicketByHashUseCase _getTicketByHashUseCase;
        private readonly InvalidateTicketUseCase _invalidateTicketUseCase;

        public TicketsController(
            GetTicketByHashUseCase getTicketByHashUseCase,
            InvalidateTicketUseCase invalidateTicketUseCase)
        {
            _getTicketByHashUseCase = getTicketByHashUseCase;
            _invalidateTicketUseCase = invalidateTicketUseCase;
        }

        /// <summary>GET /tickets/{hash} — Consulta ingresso pelo hash único (Must Have)</summary>
        [HttpGet("{hash}")]
        public async Task<IActionResult> ObterPorHash(string hash)
        {
            try
            {
                var ingresso = await _getTicketByHashUseCase.ExecutarAsync(hash);
                return Ok(new
                {
                    ingresso.Id,
                    ingresso.Hash,
                    ingresso.UsuarioId,
                    ingresso.EventoId,
                    Status = ingresso.Status.ToString()
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Erro = ex.Message });
            }
        }

        /// <summary>PATCH /tickets/{id}/invalidate — Invalida ingresso (Should Have, ex: evento cancelado)</summary>
        [HttpPatch("{id}/invalidate")]
        public async Task<IActionResult> Invalidar(Guid id)
        {
            try
            {
                var ingresso = await _invalidateTicketUseCase.ExecutarAsync(id);
                return Ok(new
                {
                    ingresso.Id,
                    ingresso.Hash,
                    Status = ingresso.Status.ToString()
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Erro = ex.Message });
            }
        }
    }
}
