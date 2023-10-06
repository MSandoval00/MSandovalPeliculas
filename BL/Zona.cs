using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Zona
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context=new DL.MsandovalCineContext())
                {
                    var query=context.Zonas.FromSqlRaw("ZonaGetAll").ToList();
                    result.Objects = new List<object>();
                    if (query!=null)
                    {
                        foreach (var row in query)
                        {
                            ML.Zona zona = new ML.Zona();
                            zona.IdZona = int.Parse(row.IdZona.ToString());
                            zona.Nombre = row.Nombre;
                            result.Objects.Add(zona);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct=false;
                        result.ErrorMessage = "No hay zonas por mostrar";
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
    }
}
