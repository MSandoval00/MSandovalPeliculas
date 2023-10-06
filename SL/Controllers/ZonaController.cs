using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonaController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Zona.GetAll();
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
