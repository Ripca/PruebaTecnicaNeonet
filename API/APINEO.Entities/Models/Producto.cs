using System;
using System.Collections.Generic;

namespace APINEO.Entities.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public double? Precio { get; set; }

    public int? Stock { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();
}
