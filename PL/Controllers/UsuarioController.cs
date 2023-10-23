using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsuarioController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult CambiarPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(ML.Usuario usuario, string Password)
        {
            //var passwordHash = GetSHA256Hash(Password);
            //var bcript = new Rfc2898DeriveBytes(Password, new byte[0], 1000, HashAlgorithmName.SHA256);
            //var passwordHash = bcript.GetBytes(20);
            byte[] inputBytes = Encoding.UTF8.GetBytes(Password);

            if (usuario.UserName != null)
            {
                usuario.Password = inputBytes;
                ML.Result result = BL.Usuario.Add(usuario);
                if (result.Correct)
                {
                    ViewBag.Mensaje = "Usuario Agregado correctamente";
                }
                else
                {
                    ViewBag.Mensaje = "No se agrego el usuario";
                }
                return PartialView("Modal");

            }
            else
            {
                ML.Result result = BL.Usuario.UsuarioGetByEmail(usuario.Email);
                usuario = (ML.Usuario)result.Object;
                var storedPassword = usuario.Password;
                if (inputBytes.SequenceEqual(storedPassword))
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }


        [HttpPost]

        public IActionResult CambiarPassword(string Email)
        {
            //llamar al metodo
            string emailOrigen = "msandovalg00@gmail.com";

            MailMessage mailMessage = new MailMessage(emailOrigen, Email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
            mailMessage.IsBodyHtml = true;

            string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "Email.html"));


            mailMessage.Body = contenidoHTML;
            string url = "http://localhost:5101/Usuario/NewPassword?Email=" + HttpUtility.UrlEncode(Email);

            mailMessage.Body = mailMessage.Body.Replace("{Url}", url);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "ptyqkrxifzsuvyox");

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

            ViewBag.Modal = "show";
            ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
            return View();
        }
        [HttpGet]
        public ActionResult NewPassword(string Email)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Email = Email;
            return View(usuario);
        }
        [HttpPost]
        //public ActionResult NewPassword(string password, string email)
        public ActionResult NewPassword(ML.Usuario usuario,string password,string Email)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            if (usuario.Email!=null)
            {
                ML.Result result1 = BL.Usuario.UsuarioGetByEmail(Email);
                if (result1.Correct)
                {
                    usuario = (ML.Usuario)result1.Object;
                }
            }
            usuario.Password=inputBytes;
            ML.Result result = BL.Usuario.Update(usuario);
            if (result.Correct)
            {
                ViewBag.Mensaje = "Contraseña Actualizada correctamente";
            }
            else
            {
                ViewBag.Mensaje = "No se actualizo la contraseña";
            }

            return PartialView("Modal");
        }
    }
}
