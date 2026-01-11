using System;
using System.Collections.Generic;

namespace APINEO.Entities.Models;

public partial class UsuarioRol
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Rol Rol { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
