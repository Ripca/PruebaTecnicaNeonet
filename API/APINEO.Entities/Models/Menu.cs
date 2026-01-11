using System;
using System.Collections.Generic;

namespace APINEO.Entities.Models;

public partial class Menu
{
    public int Id { get; set; }

    public int? MenuPadreId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Url { get; set; }

    public string? Icono { get; set; }

    public int? Orden { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Menu> InverseMenuPadre { get; set; } = new List<Menu>();

    public virtual Menu? MenuPadre { get; set; }

    public virtual ICollection<MenuRol> MenuRols { get; set; } = new List<MenuRol>();
}
