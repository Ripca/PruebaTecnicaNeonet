using System;
using System.Collections.Generic;

namespace APINEO.Entities.Models;

public partial class Venta
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public int ClienteId { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();
}
