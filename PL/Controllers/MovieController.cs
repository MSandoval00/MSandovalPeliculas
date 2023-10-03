using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class MovieController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            //Models.Result result = new Models.Result();
            Models.Root root=new Models.Root();
            root.results = new List<Models.Movie>();
            using (var client =new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/popular");
                var responseTask = client.GetAsync("?api_key=31e541d91f4612df7a43bcaf3cfe4be3");
                responseTask.Wait();

                var resultService=responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<Models.Root>();
                    readTask.Wait();
                    foreach (var resultMovie in readTask.Result.results)
                    {
                        root.results.Add(resultMovie);
                        //root.results.Add(resultItemList);
                    }
                }


            }

            return View(root);
        }
    }
}
