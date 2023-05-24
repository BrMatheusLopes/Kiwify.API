using Kiwify.API.Models;
using Kiwify.Core.Data.Entities;
using Kiwify.Core.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Kiwify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly string[] _approvedOrderArray = new string[] { "Approved", "Authorized", "Completed", "paid" };
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new ErrorMessage("Endereço de e-mail inválido."));

            var result = await _orderRepository.GetAllOrdersByEmail(email.ToLower());
            //var order = result.FirstOrDefault(x => x.Status.Equals("paid"));
            var order = result.FirstOrDefault(x => _approvedOrderArray.Contains(x.Status));

            return order == null
                ? BadRequest(new ErrorMessage($"Não foi encontrado nenhum pedido pago com esse endereço de e-mail."))
                : Ok(new ApiResponse<Order>(order));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Order order)
        {
            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
            if (updatedOrder == null)
            {
                return BadRequest(new ErrorMessage("Ocorreu uma falha ao atualizar o pedido no banco de dados."));
            }

            return Ok(new ApiResponse<Order>(order));
        }
    }
}
