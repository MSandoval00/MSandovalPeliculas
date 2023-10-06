using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Cine.GetAll();
            ML.Cine cine= new ML.Cine();
            cine.Cines = new List<object>();
            if (result.Correct)
            {
                cine.Cines = result.Objects;
            }
            else
            {
                ViewBag.Mensaje = result.ErrorMessage;
            }
            return View(cine);
        }
        [HttpGet]
        public IActionResult Form(int? IdCine)
        {
            
            ML.Cine cine=new ML.Cine();
            ML.Result resultCine = BL.Zona.GetAll();
            cine.Zona=new ML.Zona();
            if (IdCine!=null)//Update
            {
                ML.Result result = BL.Cine.GetById(IdCine.Value);
                if (result.Correct)
                {
                    cine = (ML.Cine)result.Object;
                    cine.Cines = resultCine.Objects;
                }
            }
            else
            {
                cine.Cines = resultCine.Objects;

            }
            return View(cine);
        }
        [HttpPost]
        public IActionResult Form(ML.Cine cine)
        {
            if (cine.IdCine == 0)//Add
            {
                ML.Result result = BL.Cine.Add(cine);
                if (result.Correct)
                {
                    ViewBag.Mensaje = "Se agrego correctamente el cine";

                }
                else
                {
                    ViewBag.Mensaje = "No se agrego el cine";
                }
            }
            else
            {
                ML.Result result = BL.Cine.Update(cine);
                if (result.Correct)
                {
                    ViewBag.Mensaje = "Se actualizo correctamente el cine";

                }
                else
                {
                    ViewBag.Mensaje = "No se actualizo el cine";
                }

            }
            return PartialView("Modal");
        }
        [HttpGet]
        public IActionResult Delete(int IdCine)
        {
            ML.Result result = BL.Cine.Delete(IdCine);
            if (result.Correct)
            {
                ViewBag.Mensaje = "Se borro el cine";
            }
            else
            {
                ViewBag.Mensaje = "No se borro el cine";
            }
            return PartialView("Modal");
        }
    }
}
