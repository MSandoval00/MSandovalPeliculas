﻿namespace ML
{
    public class Cine
    {
        public int IdCine { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public ML.Zona Zona { get; set; }
        public decimal Ventas { get; set; }
        public List<object> Cines { get; set; }
    }
}