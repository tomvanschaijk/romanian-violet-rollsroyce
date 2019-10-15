using Microsoft.AspNetCore.Mvc;

namespace RomanianVioletRollsRoyce.API.Controllers
{
    [Route("monitoring/[controller]")]
    [ApiController]
    public class AliveController : ControllerBase
    {
        [HttpGet]
        public IActionResult Alive() => Ok();
    }
}