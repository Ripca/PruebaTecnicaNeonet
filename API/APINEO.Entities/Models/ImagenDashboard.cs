using System;
using System.Collections.Generic;

namespace APINEO.Entities.Models;

public partial class ImagenDashboard
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public int? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int? Correlativo { get; set; }
}
