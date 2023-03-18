using Microsoft.AspNetCore.Mvc;

namespace Kiwify.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return NoContent();
        }
    }
}
