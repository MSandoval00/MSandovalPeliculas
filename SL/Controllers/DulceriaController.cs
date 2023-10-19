using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DulceriaController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result=BL.Dulceria.GetAll();
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
