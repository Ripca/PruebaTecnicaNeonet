using System;
using System.Collections.Generic;
using System.Linq;

namespace WEBNEO.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
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

    public class VentaHistorialDTO
    {
        public int VentaId { get; set; }
        public DateTime? Fecha { get; set; }
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public int? Cantidad { get; set; }
        public double? PrecioUnitario { get; set; }
        public double? Subtotal { get; set; }
    }



    public class ResponseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ErrorResponse
    {
        public string Mensaje { get; set; }
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
        public string RolesIds { get; set; } 
        public string? RolNombre { get; set; }
        public List<int> RolesIdsList => string.IsNullOrEmpty(RolesIds) ? new List<int>() : RolesIds.Split(',').Select(int.Parse).ToList();
    }

    public class UsuarioCreateRequest
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> RolesIds { get; set; }
    }

    public class UsuarioUpdateRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> RolesIds { get; set; }
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

    // ROLES
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }

    // VENTAS
    public class VentaDTO
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }

    // DASHBOARD
    public class ImagenDashboard
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? Correlativo { get; set; }
    }
}
