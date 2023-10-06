using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Cine
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context =new DL.MsandovalCineContext())
                {
                    var query = context.Cines.FromSqlRaw("CineGetAll").ToList();
                    result.Objects=new List<object>();
                    if (query!=null)
                    {
                        foreach (var row in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.IdCine = row.IdCine;
                            cine.Nombre = row.Nombre;
                            cine.Direccion=row.Direccion;
                            cine.Zona=new ML.Zona();
                            cine.Zona.IdZona =int.Parse(row.IdZona.ToString());
                            cine.Zona.Nombre=row.NombreZona.ToString();
                            cine.Ventas=decimal.Parse(row.Ventas.ToString());
                            result.Objects.Add(cine);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registros de cines";
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
        public static ML.Result GetById(int IdCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context = new DL.MsandovalCineContext())
                {
                    var query = context.Cines.FromSqlRaw($"CineGetById '{IdCine}'").AsEnumerable().SingleOrDefault();
                    result.Objects = new List<object>();
                    if (query!=null)
                    {
                        ML.Cine cine=new ML.Cine();
                        cine.IdCine = query.IdCine;
                        cine.Nombre=query.Nombre;
                        cine.Direccion=query.Direccion;
                        cine.Zona=new ML.Zona();
                        cine.Zona.IdZona = int.Parse(query.IdZona.ToString());
                        cine.Zona.Nombre = query.NombreZona;
                        cine.Ventas=decimal.Parse(query.Ventas.ToString());
                        result.Object=cine;
                        result.Correct = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage=ex.Message;
                result.Ex = ex;
            }
            return result;

        }
        public static ML.Result Delete(int IdCine)
        {
            ML.Result result=new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context=new DL.MsandovalCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineDelete '{IdCine}'");
                    if (query>0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct=false;
                        result.ErrorMessage = "No se pudo eliminar el cine";
                    }
                    result.Correct = true;

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage=ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context =new DL.MsandovalCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}','{cine.Direccion}','{cine.Zona.IdZona}',{cine.Ventas}");
                    if (query>=1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct=false;
                        result.ErrorMessage = "Error no se agrego el cine";
                    }
                    result.Correct=true;

                }
            }
            catch (Exception ex)
            {
                result.Correct = true;
                result.ErrorMessage=ex.Message;
                result.Ex=ex;
            }
            return result;

        }
        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MsandovalCineContext context = new DL.MsandovalCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineUpdate '{cine.IdCine}','{cine.Nombre}','{cine.Direccion}','{cine.Zona.IdZona}',{cine.Ventas}");
                    if (query>0)
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
            return result;
        }
    }
}