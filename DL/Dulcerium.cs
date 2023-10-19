using System;
using System.Collections.Generic;

namespace DL;

public partial class Dulcerium
{
    public int IdDulceria { get; set; }

    public string Nombre { get; set; } = null!;

    public double? Precio { get; set; }

    public string? Imagen { get; set; }
}
