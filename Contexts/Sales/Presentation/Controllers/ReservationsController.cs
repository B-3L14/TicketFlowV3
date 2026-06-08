//using Microsoft.AspNetCore.Mvc;
//using TicketFlow.Contexts.Sales.Application.DTOs;
//using TicketFlow.Contexts.Sales.Application.UseCases;

//namespace TicketFlow.Contexts.Sales.Presentation.Controllers
//{
//    [ApiController]
//    [Route("api/v1/reservations")]
//    public class ReservationsController : ControllerBase
//    {
//        private readonly CreateReservationUseCase _createReservationUseCase;
//        private readonly CancelReservationUseCase _cancelReservationUseCase;
//        private readonly GetReservationUseCase _getReservationUseCase;

//        public ReservationsController(
//            CreateReservationUseCase createReservationUseCase,
//            CancelReservationUseCase cancelReservationUseCase,
//            GetReservationUseCase getReservationUseCase)
//        {
//            _createReservationUseCase = createReservationUseCase;
//            _cancelReservationUseCase = cancelReservationUseCase;
//            _getReservationUseCase = getReservationUseCase;
//        }

//        /// <summary>POST /reservations — Reserva ingressos por X minutos (Should Have)</summary>
//        [HttpPost]
//        public async Task<IActionResult> CriarReserva([FromBody] CreateReservationRequest request)
//        {
//            try
//            {
//                var reserva = await _createReservationUseCase.ExecutarAsync(request);
//                return Created($"api/v1/reservations/{reserva.Id}", new
//                {
//                    reserva.Id,
//                    reserva.EventoId,
//                    reserva.ClienteId,
//                    reserva.Quantidade,
//                    reserva.Validade,
//                    Ativa = reserva.Ativa,
//                    TempoRestanteSegundos = (int)reserva.TempoRestante.TotalSeconds
//                });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { Erro = ex.Message });
//            }
//        }

//        /// <summary>DELETE /reservations/{id} — Cancela reserva / ingresso volta ao estoque (Should Have)</summary>
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> CancelarReserva(Guid id)
//        {
//            try
//            {
//                await _cancelReservationUseCase.ExecutarAsync(id);
//                return NoContent();
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new { Erro = ex.Message });
//            }
//        }

//        /// <summary>GET /reservations/{id} — Consulta reserva e tempo restante (Should Have)</summary>
//        [HttpGet("{id}")]
//        public async Task<IActionResult> ObterReserva(Guid id)
//        {
//            try
//            {
//                var reserva = await _getReservationUseCase.ExecutarAsync(id);
//                return Ok(new
//                {
//                    reserva.Id,
//                    reserva.EventoId,
//                    reserva.ClienteId,
//                    reserva.Quantidade,
//                    reserva.Validade,
//                    reserva.Ativa,
//                    TempoRestanteSegundos = (int)reserva.TempoRestante.TotalSeconds
//                });
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new { Erro = ex.Message });
//            }
//        }
//    }
//}
