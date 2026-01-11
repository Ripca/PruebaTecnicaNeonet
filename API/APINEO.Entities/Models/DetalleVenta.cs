using System;
using System.Collections.Generic;

namespace APINEO.Entities.Models;

public partial class DetalleVenta
{
    public int Id { get; set; }

    public int VentaId { get; set; }

    public int ProductoId { get; set; }

    public int? Cantidad { get; set; }

    public double? PrecioUnitario { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Venta Venta { get; set; } = null!;
}
