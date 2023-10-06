using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CineController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result=BL.Cine.GetAll();
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("")]
        public IActionResult Add([FromBody]ML.Cine cine)
        {
            var result = BL.Cine.Add(cine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut]
        [Route("{IdCine}")]
        public IActionResult Update(int IdCine, [FromBody]ML.Cine cine)
        {
            var result=BL.Cine.Update(cine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete]
        [Route("{IdCine}")]
        public IActionResult Delete(int IdCine)
        {
            ML.Cine cine=new ML.Cine();
            cine.IdCine=IdCine;
            var result=BL.Cine.Delete(cine.IdCine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Route("{IdCine}")]
        public IActionResult GetById(int IdCine)
        {
            ML.Result result=BL.Cine.GetById(IdCine);
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
