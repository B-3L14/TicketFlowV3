using Microsoft.AspNetCore.Mvc;
using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Application.UseCases;

namespace TicketFlow.Contexts.Sales.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly SimulatePaymentUseCase _simulatePaymentUseCase;
        private readonly GetPaymentUseCase _getPaymentUseCase;

        public PaymentsController(
            SimulatePaymentUseCase simulatePaymentUseCase,
            GetPaymentUseCase getPaymentUseCase)
        {
            _simulatePaymentUseCase = simulatePaymentUseCase;
            _getPaymentUseCase = getPaymentUseCase;
        }

        /// <summary>
        /// POST /payments/simulate — Simula gateway de pagamento (Must Have).
        /// Token "recusado" → pagamento recusado; qualquer outro valor válido → aprovado.
        /// Ingressos são gerados APENAS se pagamento aprovado.
        /// Controle de concorrência (lock) ativo no repositório.
        /// </summary>
        [HttpPost("simulate")]
        public async Task<IActionResult> SimularPagamento([FromBody] SimulatePaymentRequest request)
        {
            try
            {
                var (transacao, pedido) = await _simulatePaymentUseCase.ExecutarAsync(request);

                return Ok(new
                {
                    TransacaoId = transacao.Id,
                    PedidoId = pedido.Id,
                    StatusPagamento = transacao.Status.ToString(),
                    StatusPedido = pedido.Status.ToString(),
                    Valor = transacao.Valor,
                    ProcessadoEm = transacao.ProcessadoEm,
                    IngressosGerados = pedido.Ingressos.Select(i => new
                    {
                        i.Id,
                        i.Hash,
                        i.EventoId,
                        Status = i.Status.ToString()
                    })
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Erro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
        }

        /// <summary>GET /payments/{id} — Consulta status de uma transação (Must Have)</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPagamento(Guid id)
        {
            try
            {
                var transacao = await _getPaymentUseCase.ExecutarAsync(id);
                return Ok(new
                {
                    transacao.Id,
                    transacao.PedidoId,
                    Status = transacao.Status.ToString(),
                    transacao.Valor,
                    transacao.CriadoEm,
                    transacao.ProcessadoEm
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Erro = ex.Message });
            }
        }
    }
}
