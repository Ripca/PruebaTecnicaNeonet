using APINEO.Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace APINEO.DAL
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly EmpresaContext _context;

        public EmpresaRepository(EmpresaContext context)
        {
            _context = context;
        }

        // PRODUCTOS
        public async Task<List<Producto>> ListarProductos()
        {
            return await _context.Productos
                .FromSqlRaw("EXEC sp_ListarProductos")
                .ToListAsync();
        }

        public async Task<Producto> ObtenerProducto(int id)
        {
            var param = new SqlParameter("@Id", id);
            var result = await _context.Productos
                .FromSqlRaw("EXEC sp_ObtenerProductoPorId @Id", param)
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<int> CrearProducto(string nombre, double? precio, int? stock)
        {
            var nombreParam = new SqlParameter("@Nombre", nombre);
            var precioParam = new SqlParameter("@Precio", precio);
            var stockParam = new SqlParameter("@Stock", stock);
            var nuevoIdParam = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CrearProducto @Nombre, @Precio, @Stock, @NuevoId OUTPUT",
                nombreParam, precioParam, stockParam, nuevoIdParam);

            return (int)nuevoIdParam.Value;
        }

        public async Task ActualizarProducto(int id, string nombre, double? precio, int? stock)
        {
            var idParam = new SqlParameter("@Id", id);
            var nombreParam = new SqlParameter("@Nombre", nombre);
            var precioParam = new SqlParameter("@Precio", precio);
            var stockParam = new SqlParameter("@Stock", stock);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_ActualizarProducto @Id, @Nombre, @Precio, @Stock",
                idParam, nombreParam, precioParam, stockParam);
        }

        public async Task CambiarEstadoProducto(int id, int estado)
        {
            var idParam = new SqlParameter("@Id", id);
            var estadoParam = new SqlParameter("@Estado", estado);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CambiarEstadoProducto @Id, @Estado",
                idParam, estadoParam);
        }

        public async Task<List<ReporteProducto>> ObtenerReporteProductosMasVendidos()
        {
            return await _context.Database
                .SqlQueryRaw<ReporteProducto>("EXEC sp_ReporteProductosMasVendidos")
                .ToListAsync();
        }



        // CLIENTES
        public async Task<List<Cliente>> ListarClientes()
        {
            return await _context.Clientes
                .FromSqlRaw("EXEC sp_ListarClientes")
                .ToListAsync();
        }

        public async Task<Cliente> ObtenerClientePorId(int id)
        {
            var param = new SqlParameter("@Id", id);
            var result = await _context.Clientes
                .FromSqlRaw("EXEC sp_ObtenerClientePorId @Id", param)
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<Cliente> ObtenerClientePorEmail(string email)
        {
            var param = new SqlParameter("@Email", email);
            var result = await _context.Clientes
                .FromSqlRaw("EXEC sp_ObtenerClientePorEmail @Email", param)
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<int> CrearCliente(string nombre, string email)
        {
            var nombreParam = new SqlParameter("@Nombre", nombre);
            var emailParam = new SqlParameter("@Email", email);
            var nuevoIdParam = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CrearCliente @Nombre, @Email, @NuevoId OUTPUT",
                nombreParam, emailParam, nuevoIdParam);

            return (int)nuevoIdParam.Value;
        }

        public async Task ActualizarCliente(int id, string nombre, string email)
        {
            var idParam = new SqlParameter("@Id", id);
            var nombreParam = new SqlParameter("@Nombre", nombre);
            var emailParam = new SqlParameter("@Email", email);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_ActualizarCliente @Id, @Nombre, @Email",
                idParam, nombreParam, emailParam);
        }

        public async Task CambiarEstadoCliente(int id, int estado)
        {
            var idParam = new SqlParameter("@Id", id);
            var estadoParam = new SqlParameter("@Estado", estado);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CambiarEstadoCliente @Id, @Estado",
                idParam, estadoParam);
        }

        public async Task<List<ReporteCliente>> ObtenerReporteTotalVendidoCliente()
        {
            return await _context.Database
                .SqlQueryRaw<ReporteCliente>("EXEC sp_ReporteTotalVendidoPorCliente")
                .ToListAsync();
        }

        // VENTAS
        public async Task<int> RegistrarVenta(Venta venta)
        {
            var clienteIdParam = new SqlParameter("@ClienteId", venta.ClienteId);
            var nuevoIdParam = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegistrarVenta @ClienteId, @NuevoId OUTPUT",
                clienteIdParam, nuevoIdParam);

            return (int)nuevoIdParam.Value;
        }

        public async Task RegistrarDetalleVenta(DetalleVenta detalle)
        {
            var ventaIdParam = new SqlParameter("@VentaId", detalle.VentaId);
            var productoIdParam = new SqlParameter("@ProductoId", detalle.ProductoId);
            var cantidadParam = new SqlParameter("@Cantidad", detalle.Cantidad);
            var precioParam = new SqlParameter("@PrecioUnitario", detalle.PrecioUnitario);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegistrarDetalleVenta @VentaId, @ProductoId, @Cantidad, @PrecioUnitario",
                ventaIdParam, productoIdParam, cantidadParam, precioParam);
        }



        public async Task<List<VentaDTO>> ObtenerVentasPorCliente(int clienteId)
        {
            var param = new SqlParameter("@ClienteId", clienteId);
            return await _context.Database
                .SqlQueryRaw<VentaDTO>("EXEC usp_ObtenerVentasPorCliente @ClienteId", param)
                .ToListAsync();
        }

        public async Task<List<VentaHistorialDTO>> ObtenerDetalleVenta(int ventaId)
        {
            var param = new SqlParameter("@VentaId", ventaId);
            return await _context.Database
                .SqlQueryRaw<VentaHistorialDTO>("EXEC sp_DetalleVenta @VentaId", param)
                .ToListAsync();
        }

        public async Task<List<VentaDTO>> ListarVentas()
        {
            return await _context.Database
                .SqlQueryRaw<VentaDTO>("EXEC sp_ListarVentas")
                .ToListAsync();
        }

        public async Task<List<object>> ListarTodasLasVentas()
        {
            var result = await _context.Database
                .SqlQueryRaw<dynamic>("EXEC sp_ListarTodasLasVentas")
                .ToListAsync();
            return result.Cast<object>().ToList();
        }

        // AUTENTICACIÓN
        public async Task<UsuarioDTO> ValidarUsuario(string email, string password)
        {
            var emailParam = new SqlParameter("@Email", email);
            var passwordParam = new SqlParameter("@Password", password);

            var result = await _context.Database
                .SqlQueryRaw<UsuarioDTO>("EXEC sp_ValidarUsuario @Email, @Password", emailParam, passwordParam)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<List<MenuDTO>> ObtenerMenusPorUsuario(int usuarioId)
        {
            var param = new SqlParameter("@UsuarioId", usuarioId);
            return await _context.Database
                .SqlQueryRaw<MenuDTO>("EXEC sp_ObtenerMenusPorUsuario @UsuarioId", param)
                .ToListAsync();
        }

        // USUARIOS
        public async Task<List<UsuarioDTO>> ListarUsuarios()
        {
            return await _context.Database
                .SqlQueryRaw<UsuarioDTO>("EXEC sp_ListarUsuarios")
                .ToListAsync();
        }

        public async Task<UsuarioDTO> ObtenerUsuarioPorId(int id)
        {
            var param = new SqlParameter("@Id", id);
            var result = await _context.Database
                .SqlQueryRaw<UsuarioDTO>("EXEC sp_ObtenerUsuarioPorId @Id", param)
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<int> CrearUsuario(UsuarioCreateRequest request)
        {
            var nombreParam = new SqlParameter("@Nombre", request.Nombre);
            var emailParam = new SqlParameter("@Email", request.Email);
            var passwordParam = new SqlParameter("@Password", request.Password);
            var rolesIdsParam = new SqlParameter("@RolesIds", string.Join(",", request.RolesIds));
            var nuevoIdParam = new SqlParameter("@NuevoId", SqlDbType.Int) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CrearUsuario @Nombre, @Email, @Password, @RolesIds, @NuevoId OUTPUT",
                nombreParam, emailParam, passwordParam, rolesIdsParam, nuevoIdParam);

            return (int)nuevoIdParam.Value;
        }

        public async Task ActualizarUsuario(UsuarioUpdateRequest request)
        {
            var idParam = new SqlParameter("@Id", request.Id);
            var nombreParam = new SqlParameter("@Nombre", request.Nombre);
            var emailParam = new SqlParameter("@Email", request.Email);
            var passwordParam = new SqlParameter("@Password", (object)request.Password ?? DBNull.Value);
            var rolesIdsParam = new SqlParameter("@RolesIds", string.Join(",", request.RolesIds));

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_ActualizarUsuario @Id, @Nombre, @Email, @Password, @RolesIds",
                idParam, nombreParam, emailParam, passwordParam, rolesIdsParam);
        }

        public async Task CambiarEstadoUsuario(int id, int estado)
        {
            var idParam = new SqlParameter("@Id", id);
            var estadoParam = new SqlParameter("@Estado", estado);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CambiarEstadoUsuario @Id, @Estado",
                idParam, estadoParam);
        }

        // MENUS
        public async Task<List<MenuDTO>> ListarMenus()
        {
            return await _context.Database
                .SqlQueryRaw<MenuDTO>("EXEC sp_ListarMenus")
                .ToListAsync();
        }

        public async Task<MenuDTO> ObtenerMenuPorId(int id)
        {
            var param = new SqlParameter("@Id", id);
            var result = await _context.Database
                .SqlQueryRaw<MenuDTO>("EXEC sp_ObtenerMenuPorId @Id", param)
                .ToListAsync();
            return result.FirstOrDefault();
        }

        // ROLES
        public async Task<List<Rol>> ListarRoles()
        {
            return await _context.Roles
                .FromSqlRaw("EXEC sp_ListarRoles")
                .ToListAsync();
        }

        // DASHBOARD IMAGES
        public async Task<List<ImagenDashboard>> ListarImagenesDashboard()
        {
            return await _context.Database
                .SqlQueryRaw<ImagenDashboard>("EXEC sp_ListarImagenesDashboard")
                .ToListAsync();
        }

        public async Task RegistrarImagenDashboard(string url, int correlativo)
        {
            var urlParam = new SqlParameter("@Url", url);
            var correlativoParam = new SqlParameter("@Correlativo", correlativo);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegistrarImagenDashboard @Url, @Correlativo",
                urlParam, correlativoParam);
        }

        public async Task CambiarEstadoImagenDashboard(int id, int estado)
        {
            var idParam = new SqlParameter("@Id", id);
            var estadoParam = new SqlParameter("@Estado", estado);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CambiarEstadoImagenDashboard @Id, @Estado",
                idParam, estadoParam);
        }
    }
}
