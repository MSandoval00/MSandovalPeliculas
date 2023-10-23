using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result UsuarioGetByEmail(string Email)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context=new DL.MsandovalCineContext())
                {
                    var query =context.Usuarios.FromSqlRaw($"UsuariGetByEmail '{Email}'").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();
                   if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.UserName=query.UserName;
                        usuario.Email=query.Email;
                        usuario.Nombre=query.Nombre;
                        usuario.Password = query.Password;
                        result.Object = usuario;
                        result.Correct = true;
                        //usuario.Password = query.Password;
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex=ex;
            }
            return result;


        }
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result=new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context=new DL.MsandovalCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.UserName}','{usuario.Email}','{usuario.Nombre}', @Password",new SqlParameter("@Password",usuario.Password));
                    if (query>0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se inserto";
                    }
                    result.Correct = true;

                }
            }
            catch (Exception ex)
            {
                result.Correct=false;
                result.ErrorMessage=ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context=new DL.MsandovalCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioUpdatePassword '{usuario.Email}', @Password", new SqlParameter("@Password", usuario.Password));
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error no se actualizo el cine";
                    }
                    result.Correct = true;

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result ;

        }
    }
}
