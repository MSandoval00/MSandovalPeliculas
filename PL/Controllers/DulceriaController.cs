
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;

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
        public ActionResult GeneratePDF()
        {
            ML.Venta carrito=new ML.Venta();
            carrito.Carrito=new List<object>();
            GetCarrito(carrito);

            //crea un archivo pdf en la ubicación temporal

            string rutaPDF=Path.GetTempFileName()+".pdf";
            using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(rutaPDF)))
            {
                using (Document document = new Document(pdfDocument))
                {
                    document.Add(new Paragraph("Resumen de Compra"));//Titulo del PDF

                    // Crear la tabla para mostrar la lista de objetos
                    Table table = new Table(5); // Mostrar las columnas
                    table.SetWidth(UnitValue.CreatePercentValue(100)); // Ancho de la tabla al 100% del documento

                    // Encabezados de las celdas para la tabla
                    table.AddHeaderCell("Imagen");
                    table.AddHeaderCell("Nombre");
                    table.AddHeaderCell("Precio Unidad");
                    table.AddHeaderCell("Subtotal");
                    table.AddHeaderCell("Cantidad");


                    foreach (ML.Dulceria dulceria in carrito.Carrito)
                    {
                        byte[] imageBytes = Convert.FromBase64String(dulceria.Imagen);
                        ImageData data = ImageDataFactory.Create(imageBytes);
                        Image image = new Image(data);
                        table.AddCell(image.SetWidth(50).SetHeight(50));

                        table.AddCell(dulceria.Nombre);
                        table.AddCell(dulceria.Precio.ToString());
                        table.AddCell((dulceria.Precio*dulceria.Cantidad).ToString());
                        table.AddCell(dulceria.Cantidad.ToString());
                        //dulceria.precioTotal+=dulceria.Precio*dulceria.Cantidad;
                    }
                    // Añadir la tabla al documento
                    document.Add(table);
                    //document.Add(new Paragraph("Total de compra:"));
                }
            }

            // Leer el archivo PDF como un arreglo de bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaPDF);

            // Eliminar el archivo temporal
            System.IO.File.Delete(rutaPDF);
            HttpContext.Session.Remove("Carrito");

            // Descargar el archivo PDF
            return new FileStreamResult(new MemoryStream(fileBytes), "application/pdf")
            {
                FileDownloadName = "ReporteProductos.pdf"
            };

        }

    }
}
