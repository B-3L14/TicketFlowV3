//using Microsoft.AspNetCore.Mvc;
//using TicketFlow.Contexts.Sales.Application.DTOs;
//using TicketFlow.Contexts.Sales.Application.UseCases;
//using TicketFlow.Contexts.Sales.Domain.Enums;

//namespace TicketFlow.Contexts.Sales.Presentation.Controllers
//{
//    [ApiController]
//    [Route("api/v1/orders")]
//    public class CheckoutController : ControllerBase
//    {
//        private readonly CheckoutUseCase _checkoutUseCase;

//        public CheckoutController(CheckoutUseCase checkoutUseCase)
//        {
//            _checkoutUseCase = checkoutUseCase;
//        }

//        // Mantido para compatibilidade com o fluxo completo de uma só chamada
//        [HttpPost("checkout")]
//        public async Task<IActionResult> RealizarCheckout([FromBody] CheckoutRequest request)
//        {
//            try
//            {
//                var pedido = await _checkoutUseCase.ExecutarAsync(request);

//                if (pedido.Status == OrderStatus.Cancelado)
//                {
//                    return BadRequest(new
//                    {
//                        Mensagem = "Pagamento recusado ou falha na disponibilidade. O pedido foi cancelado."
//                    });
//                }

//                return Ok(new
//                {
//                    PedidoId = pedido.Id,
//                    Status = pedido.Status.ToString(),
//                    ValorTotal = pedido.ValorTotal,
//                    IngressosGerados = pedido.Ingressos.Select(i => new
//                    {
//                        i.Id,
//                        i.Hash,
//                        i.EventoId
//                    })
//                });
//            }
//            catch (InvalidOperationException ex)
//            {
//                return BadRequest(new { Erro = ex.Message });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { Erro = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    Erro = "Ocorreu um conflito ao processar sua compra. O ingresso pode ter esgotado. Tente novamente.",
//                    Detalhe = ex.Message
//                });
//            }
//        }
//    }
//}
