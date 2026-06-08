using Microsoft.AspNetCore.Mvc;
using TicketFlow.Contexts.Sales.Application.DTOs;
using TicketFlow.Contexts.Sales.Application.UseCases;

namespace TicketFlow.Contexts.Sales.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderUseCase _createOrderUseCase;
        private readonly AddItemUseCase _addItemUseCase;
        private readonly RemoveItemUseCase _removeItemUseCase;
        private readonly UpdateOrderStatusUseCase _updateOrderStatusUseCase;
        private readonly OrderHistoryUseCase _orderHistoryUseCase;

        public OrdersController(
            CreateOrderUseCase createOrderUseCase,
            AddItemUseCase addItemUseCase,
            RemoveItemUseCase removeItemUseCase,
            UpdateOrderStatusUseCase updateOrderStatusUseCase,
            OrderHistoryUseCase orderHistoryUseCase)
        {
            _createOrderUseCase = createOrderUseCase;
            _addItemUseCase = addItemUseCase;
            _removeItemUseCase = removeItemUseCase;
            _updateOrderStatusUseCase = updateOrderStatusUseCase;
            _orderHistoryUseCase = orderHistoryUseCase;
        }

        /// <summary>POST /orders — Cria pedido com status Pendente (Must Have)</summary>
        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] CreateOrderRequest request)
        {
            try
            {
                var pedido = await _createOrderUseCase.ExecutarAsync(request);
                return Created($"api/v1/orders/{pedido.Id}", new
                {
                    pedido.Id,
                    pedido.ClienteId,
                    Status = pedido.Status.ToString(),
                    pedido.ValorTotal
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
        }

        /// <summary>POST /orders/{orderId}/items — Adiciona item ao carrinho (Must Have)</summary>
        [HttpPost("{orderId}/items")]
        public async Task<IActionResult> AdicionarItem(Guid orderId, [FromBody] AddItemRequest request)
        {
            try
            {
                var pedido = await _addItemUseCase.ExecutarAsync(orderId, request);
                return Ok(new
                {
                    pedido.Id,
                    Status = pedido.Status.ToString(),
                    pedido.ValorTotal,
                    Itens = pedido.Itens.Select(i => new
                    {
                        i.Id,
                        i.EventoId,
                        i.Quantidade,
                        i.PrecoUnitario,
                        i.ValorTotal
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
            catch (ArgumentException ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
        }

        /// <summary>DELETE /orders/{orderId}/items/{itemId} — Remove item do carrinho (Must Have)</summary>
        [HttpDelete("{orderId}/items/{itemId}")]
        public async Task<IActionResult> RemoverItem(Guid orderId, Guid itemId)
        {
            try
            {
                await _removeItemUseCase.ExecutarAsync(orderId, itemId);
                return NoContent();
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

        /// <summary>PATCH /orders/{id}/status — Transita o status do pedido (Must Have)</summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                var pedido = await _updateOrderStatusUseCase.ExecutarAsync(id, request);
                return Ok(new
                {
                    pedido.Id,
                    Status = pedido.Status.ToString(),
                    pedido.ValorTotal,
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
            catch (ArgumentException ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
        }

        /// <summary>GET /orders/history?clienteId={id} — Histórico de compras (Should Have)</summary>
        [HttpGet("history")]
        public async Task<IActionResult> ObterHistorico([FromQuery] Guid clienteId)
        {
            try
            {
                var pedidos = await _orderHistoryUseCase.ExecutarAsync(clienteId);
                return Ok(pedidos.Select(p => new
                {
                    p.Id,
                    p.ClienteId,
                    Status = p.Status.ToString(),
                    p.ValorTotal,
                    Itens = p.Itens.Select(i => new
                    {
                        i.Id,
                        i.EventoId,
                        i.Quantidade,
                        i.PrecoUnitario
                    }),
                    Ingressos = p.Ingressos.Select(i => new
                    {
                        i.Id,
                        Hash = i.Hash.Valor,
                        i.EventoId,
                        Status = i.Status.ToString()
                    })
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
        }
    }
}
