using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(ML.Usuario usuario, string Password)
        {
            var bcript = new Rfc2898DeriveBytes(Password, new byte[0], 1000, HashAlgorithmName.SHA256);
            var passwordHash = bcript.GetBytes(20);

            if (usuario.UserName != null)
            {
                //usuario.Password = passwordHash;
                ML.Result result = BL.Usuario.Add(usuario);
                return View();

            }
            else
            {
                ML.Result result = BL.Usuario.UsuarioGetByEmail(usuario.Email);
                usuario = (ML.Usuario)result.Object;
                var storedPassword = usuario.Password;
                if (passwordHash == storedPassword)
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            //var passwordHash = GetSHA256Hash(Password);
            //var passwordHashBytes = Encoding.UTF8.GetBytes(passwordHash);
            //if (usuario.UserName != null)
            //{
            //    ML.Result result = BL.Usuario.Add(usuario);
            //    return View();
            //}
            //else
            //{
            //    ML.Result result = BL.Usuario.UsuarioGetByEmail(usuario.Email);
            //    usuario = (ML.Usuario)result.Object;
            //    var storedPassword = usuario.Password;


            //    if (passwordHashBytes.SequenceEqual(storedPassword))
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            return View();
        }
    public static string GetSHA256Hash(string Password)
        {
            using (SHA256 sha256 =SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
        }
    }
}
