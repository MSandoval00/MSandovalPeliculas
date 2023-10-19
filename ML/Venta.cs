using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Venta
    {
        public int Cantidad { get; set; }
        public float? Total { get; set; }
        public float? SubTotal { get; set; }
        public List<object> Carrito { get; set; }
    }
}
