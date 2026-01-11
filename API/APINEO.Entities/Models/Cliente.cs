using System;
using System.Collections.Generic;

namespace APINEO.Entities.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Email { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
