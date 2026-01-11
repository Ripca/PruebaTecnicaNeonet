namespace APINEO.Entities.Models
{
    // REPORTES
    public class ReporteCliente
    {
        public string Cliente { get; set; }
        public double TotalVendido { get; set; }
    }

    public class ReporteProducto
    {
        public string Producto { get; set; }
        public int CantidadTotal { get; set; }
    }

    // VENTAS
    public class VentaHistorialDTO
    {
        public int VentaId { get; set; }
        public System.DateTime? Fecha { get; set; }
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public int? Cantidad { get; set; }
        public double? PrecioUnitario { get; set; }
        public double? Subtotal { get; set; }
    }

    public class VentaRequest
    {
        public int ClienteId { get; set; }
        public List<DetalleVentaRequest> Detalles { get; set; }
    }

    public class DetalleVentaRequest
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
    }

    // AUTENTICACIÓN
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? TokenCaptcha { get; set; }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public List<int> RolesIds { get; set; }
        public string Token { get; set; }
    }

    // USUARIOS
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string RolesIds { get; set; } // IDs concatenados del SP: "1,2"
        public string? RolNombre { get; set; } // For legacy/single-role compatibility
        public List<int> RolesIdsList => string.IsNullOrEmpty(RolesIds) ? new List<int>() : RolesIds.Split(',').Select(int.Parse).ToList();
    }

    public class UsuarioCreateRequest
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> RolesIds { get; set; } // IDs de roles a asignar
    }

    public class UsuarioUpdateRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public List<int> RolesIds { get; set; } // IDs de roles a asignar
    }

    // MENUS
    public class MenuDTO
    {
        public int Id { get; set; }
        public int? MenuPadreId { get; set; }
        public string? Nombre { get; set; }
        public string? Url { get; set; }
        public string? Icono { get; set; }
        public int? Orden { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public List<MenuDTO>? SubMenus { get; set; }
    }

    public class VentaDTO
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}

