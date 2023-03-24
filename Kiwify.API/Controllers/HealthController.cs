using Microsoft.AspNetCore.Mvc;

namespace Kiwify.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index([FromQuery(Name = "echo")] string? echo = null)
        {
            return string.IsNullOrEmpty(echo) 
                ? NoContent() 
                : Ok(new { Response = echo });
        }
    }
}
