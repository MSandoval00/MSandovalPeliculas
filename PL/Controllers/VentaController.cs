using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class VentaController : Controller
    {
        public IActionResult Catalogo()
        {
            ML.Result result = new ML.Result();
            ML.Dulceria dulceria = new ML.Dulceria();
            dulceria.Dulcerias = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5122/api/");
                var responseTask = client.GetAsync("Dulceria");
                responseTask.Wait();
                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resulDulceria in readTask.Result.Objects)
                    {
                        ML.Dulceria resultItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(resulDulceria.ToString());
                        dulceria.Dulcerias.Add(resultItem);

                    }
                }
            }
            return View(dulceria);
        }
    }
}
