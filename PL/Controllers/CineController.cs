using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            //ML.Result result = BL.Cine.GetAll();
            //ML.Cine cine= new ML.Cine();
            //cine.Cines = new List<object>();
            //if (result.Correct)
            //{
            //    cine.Cines = result.Objects;
            //}
            //else
            //{
            //    ViewBag.Mensaje = result.ErrorMessage;
            //}
            ML.Result result=new ML.Result();
            ML.Cine cine=new ML.Cine();
            cine.Cines = new List<object>();
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5122/api/");
                var responseTask = client.GetAsync("Cine");
                responseTask.Wait();
                var resultService=responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resultCine in readTask.Result.Objects )
                    {
                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultCine.ToString());
                        cine.Cines.Add(resultItemList);
                    }
                }
            }
            return View(cine);
        }
        [HttpGet]
        public IActionResult Form(int? IdCine)
        {

            //ML.Cine cine=new ML.Cine();
            //ML.Result resultCine = BL.Zona.GetAll();
            //cine.Zona=new ML.Zona();
            //if (IdCine!=null)//Update
            //{
            //    ML.Result result = BL.Cine.GetById(IdCine.Value);
            //    if (result.Correct)
            //    {
            //        cine = (ML.Cine)result.Object;
            //        cine.Cines = resultCine.Objects;
            //    }
            //}
            //else
            //{
            //    cine.Cines = resultCine.Objects;

            //}
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();
            ML.Result resultZona = BL.Zona.GetAll();
            
            if (IdCine != null)//Update
            {
                using (var client=new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5122/api/");
                    var responseTask = client.GetAsync("Cine/" + IdCine.Value);
                    responseTask.Wait();

                    var resultService = responseTask.Result;
                    if (resultService.IsSuccessStatusCode)
                    {
                        var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Cine resultCine = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(readTask.Result.Object.ToString());
                        cine=resultCine;
                    }
                }
                if (resultZona.Correct)
                {
                    cine.Cines = resultZona.Objects;
                }
            }
            else
            {
                cine.Cines = resultZona.Objects;

            }
            return View(cine);
        }
        [HttpPost]
        public IActionResult Form(ML.Cine cine)
        {
            //if (cine.IdCine == 0)//Add
            //{
            //    ML.Result result = BL.Cine.Add(cine);
            //    if (result.Correct)
            //    {
            //        ViewBag.Mensaje = "Se agrego correctamente el cine";

            //    }
            //    else
            //    {
            //        ViewBag.Mensaje = "No se agrego el cine";
            //    }
            //}
            //else
            //{
            //    ML.Result result = BL.Cine.Update(cine);
            //    if (result.Correct)
            //    {
            //        ViewBag.Mensaje = "Se actualizo correctamente el cine";

            //    }
            //    else
            //    {
            //        ViewBag.Mensaje = "No se actualizo el cine";
            //    }

            //}
            if (cine.IdCine == 0)//Add
            {
                using (var client =new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5122/api/Cine");
                    var postTask = client.PostAsJsonAsync(client.BaseAddress, cine);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se agrego correctamente el cine";

                    }
                    else
                    {
                        ViewBag.Mensaje = "No se agrego el cine";
                    }

                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5122/api/");
                    var postTask = client.PutAsJsonAsync("Cine/"+ cine.IdCine, cine);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se actualizo correctamente el cine";

                    }
                    else
                    {
                        ViewBag.Mensaje = "No se actualizo el cine";
                    }

                }

            }
            return PartialView("Modal");
        }
        [HttpGet]
        public IActionResult Delete(int IdCine)
        {
            //ML.Result result = BL.Cine.Delete(IdCine);
            //if (result.Correct)
            //{
            //    ViewBag.Mensaje = "Se borro el cine";
            //}
            //else
            //{
            //    ViewBag.Mensaje = "No se borro el cine";
            //}
            ML.Result resultCine = new ML.Result();
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5122/api/");
                var postTask = client.DeleteAsync("Cine/" + IdCine);
                postTask.Wait();
                var result= postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se borro correctamente el cine";

                }
                else
                {
                    ViewBag.Mensaje = "No se borro el cine";
                }
            }
            return PartialView("Modal");
        }
    }
}
