using Kiwify.API.Services;
using Microsoft.AspNetCore.Mvc;
using PerfectPay.Library.Models;
using System.Text.Json;

namespace Kiwify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfectPayController : ControllerBase
    {
        private readonly PerfectPayPaymentHandler _paymentHandler;
        private readonly IConfiguration _configuration;

        public PerfectPayController(PerfectPayPaymentHandler paymentHandler, IConfiguration configuration)
        {
            _paymentHandler = paymentHandler;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] object payload)
        {
            if (payload == null)
                return BadRequest(new { error = "Object body cannot be null or empty" });

            var order = JsonSerializer.Deserialize<PerfectPayOrder>(payload.ToString()!);
            if (order == null)
                return BadRequest(new { error = "Invalid json format" });

            await _paymentHandler.Handler(order);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get([FromQuery(Name = "pass")] string pass)
        {
            return pass.Equals(AppDomain.CurrentDomain.FriendlyName) 
                ? Ok(_paymentHandler.Customers) 
                : Unauthorized();
        }

        private bool IsValidToken(string orderToken)
        {
            var secretToken =
                Environment.GetEnvironmentVariable("SecretToken")
                ?? _configuration.GetSection("PerfectPayConfiguration").Get<PerfectPayConfiguration>()?.SecretToken;

            return secretToken != null 
                && secretToken.Equals(orderToken);
        }
    }
}
