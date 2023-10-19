using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Dulceria
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context=new DL.MsandovalCineContext())
                {
                    var query=context.Dulceria.FromSqlRaw("DulceriaGetAll").ToList();
                    result.Objects = new List<object>();
                    if (query!=null)
                    {
                        foreach (var row in query)
                        {
                            ML.Dulceria dulceria = new ML.Dulceria();
                            dulceria.IdDulceria=int.Parse(row.IdDulceria.ToString());
                            dulceria.Nombre = row.Nombre;
                            dulceria.Precio = float.Parse(row.Precio.ToString());
                            dulceria.Imagen= row.Imagen;
                            result.Objects.Add(dulceria);

                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registro en la tabla ";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct=false;
                result.ErrorMessage=ex.Message;
                result.Ex=ex;
            }
            return result;
        }
        public static  ML.Result GetById(int IdDulceria)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context=new DL.MsandovalCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw($"DulceriaGetById '{IdDulceria}'").AsEnumerable().SingleOrDefault();
                    result.Object=new List<object>();
                    if (query != null)
                    {
                        ML.Dulceria dulceria = new ML.Dulceria();
                        dulceria.IdDulceria= query.IdDulceria;
                        dulceria.Nombre=query.Nombre;
                        dulceria.Precio = float.Parse(query.Precio.ToString());
                        dulceria.Imagen=query.Imagen;
                        result.Object=dulceria;
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage= ex.Message;
                result.Ex= ex;
            }
            return result;

        }
    }
}
