using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;

namespace PL.Controllers
{
    public class MovieController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            
            Models.Root root = new Models.Root();
            root.results = new List<Models.Movie>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/popular");
                var responseTask = client.GetAsync("?api_key=31e541d91f4612df7a43bcaf3cfe4be3");
                responseTask.Wait();

                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<Models.Root>();
                    readTask.Wait();
                    foreach (var resultMovie in readTask.Result.results)
                    {
                        root.results.Add(resultMovie);

                    }
                }


            }

            return View(root);
        }
        [HttpGet]
        public ActionResult GetAllSin(int? page)
        {

            Models.MovieS movieS = new Models.MovieS();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/popular");
                //var responseTask = client.GetAsync("");
                string url = "";
                //if (page == null)
                //{
                    page = page==null ? 1:page;
                    url = "?api_key=31e541d91f4612df7a43bcaf3cfe4be3&page=" + page;
                //}
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();
                    movieS.Movies = new List<object>();
                    foreach (var resultItem in resultJSON.results)
                    {
                        Models.MovieS movieItem = new Models.MovieS();
                   
                        movieItem.id = resultItem.id;
                        movieItem.title = resultItem.title;
                        movieItem.release_date = resultItem.release_date;
                        movieItem.original_title = resultItem.original_title;
                        movieItem.overview = resultItem.overview;
                        movieItem.popularity = resultItem.popularity;
                        movieItem.backdrop_path = "https://www.themoviedb.org/t/p/w300_and_h450_bestv2/" + resultItem.backdrop_path;
                        movieS.Movies.Add(movieItem);

                    }
                }


            }
            ViewBag.page = page;
            return View(movieS);
        }
        
        
        public ActionResult AddFavorite(int IdMovie, bool Favorite)
        {
            
            using (var client = new HttpClient())
            {
  
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/");
                var json = new
                {
                    media_type = "movie",
                    media_id = IdMovie,
                    favorite = Favorite
                };
                var postTask = client.PostAsJsonAsync("20522403/favorite?session_id=4daa41461d611b36e69708ce658629a4f87c3595&api_key=31e541d91f4612df7a43bcaf3cfe4be3",json);
                postTask.Wait();
                var resultMovie=postTask.Result;
                if (Favorite)
                {
                    if (resultMovie.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Agregado a favoritos";
                    }
                }
                else
                {
                    if (resultMovie.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Eliminado de favoritos";
                    }
                }
                
                return View("Modal");
            }
        }

        public ActionResult GetAllFavorite(int? page)
        {
            Models.MovieS movieS = new Models.MovieS();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

                string url = "";
                if (page == null)
                {
                    url = "account/20522454/favorite/movies?api_key=31e541d91f4612df7a43bcaf3cfe4be3&session_id=4daa41461d611b36e69708ce658629a4f87c3595";
                }



                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();
                    movieS.Movies = new List<object>();
                    foreach (var resultItem in resultJSON.results)
                    {
                        Models.MovieS movieItem = new Models.MovieS();
                        movieItem.id = resultItem.id;
                        movieItem.title = resultItem.title;
                        movieItem.release_date = resultItem.release_date;
                        movieItem.original_title = resultItem.original_title;
                        movieItem.overview = resultItem.overview;
                        movieItem.popularity = resultItem.popularity;
                        movieItem.backdrop_path = "https://www.themoviedb.org/t/p/w300_and_h450_bestv2/" + resultItem.backdrop_path;
                        movieS.Movies.Add(movieItem);

                    }
                }
            }
            ViewBag.page = page;
            return View(movieS);
        }

    }
}

