using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DulceriaController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = new ML.Result();
            ML.Dulceria dulceria=new ML.Dulceria();
            dulceria.Dulcerias = new List<object>();
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5122/api/");
                var responseTask = client.GetAsync("Dulceria");
                responseTask.Wait();
                var resultService=responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resulDulceria in readTask.Result.Objects)
                    {
                        ML.Dulceria resultItem=Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(resulDulceria.ToString());
                        dulceria.Dulcerias.Add(resultItem);

                    }
                }
            }
            return View(dulceria);
        }
        public static byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();
            byte[] bytes = new byte[fileStream.Length];
            fileStream.ReadByte();
            fileStream.Read(bytes, 0, (int)bytes.Length);
            return bytes;

        }
        public IActionResult AddCarrito(int IdDulceria)
        {
            bool existe = false;
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            ML.Result result = BL.Dulceria.GetById(IdDulceria);
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                if (result.Correct)
                {
                    ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                    dulceria.Cantidad = 1;
                    carrito.Carrito.Add(dulceria);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));

                }
            }
            else
            {
                ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                GetCarrito(carrito);//Crear y llamar la función
                foreach (ML.Dulceria item in carrito.Carrito)
                {
                    if (dulceria.IdDulceria == item.IdDulceria)
                    {
                        item.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe == true)
                {
                    
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                    
                }
                else
                {
                    dulceria.Cantidad = 1;
                    carrito.Carrito.Add(dulceria);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }
            return RedirectToAction("Carrito"); //Referenciar getall dulceria en view
        }
        public ML.Venta GetCarrito(ML.Venta carrito)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));
            foreach (var obj in ventaSession)
            {
                ML.Dulceria objDulceria = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(obj.ToString());
                carrito.Carrito.Add(objDulceria);
            }
            return carrito;

        }
        public IActionResult Carrito()
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                return View(carrito);
            }
        }

    }
}
