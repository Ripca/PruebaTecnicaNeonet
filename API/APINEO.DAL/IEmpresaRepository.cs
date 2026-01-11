using APINEO.Entities.Models;

namespace APINEO.DAL
{
    public interface IEmpresaRepository
    {
        // PRODUCTOS
        Task<List<Producto>> ListarProductos();
        Task<Producto> ObtenerProducto(int id);
        Task<int> CrearProducto(string nombre, double? precio, int? stock);
        Task ActualizarProducto(int id, string nombre, double? precio, int? stock);
        Task CambiarEstadoProducto(int id, int estado);
        Task<List<ReporteProducto>> ObtenerReporteProductosMasVendidos();


        // CLIENTES
        Task<List<Cliente>> ListarClientes();
        Task<Cliente> ObtenerClientePorId(int id);
        Task<Cliente> ObtenerClientePorEmail(string email);
        Task<int> CrearCliente(string nombre, string email);
        Task ActualizarCliente(int id, string nombre, string email);
        Task CambiarEstadoCliente(int id, int estado);
        Task<List<ReporteCliente>> ObtenerReporteTotalVendidoCliente();

        // VENTAS
        Task<int> RegistrarVenta(Venta venta);
        Task RegistrarDetalleVenta(DetalleVenta detalle);

        Task<List<VentaDTO>> ObtenerVentasPorCliente(int clienteId);
        Task<List<VentaDTO>> ListarVentas();
        Task<List<VentaHistorialDTO>> ObtenerDetalleVenta(int ventaId);
        Task<List<object>> ListarTodasLasVentas();

        // AUTENTICACIÓN
        Task<UsuarioDTO> ValidarUsuario(string email, string password);
        Task<List<MenuDTO>> ObtenerMenusPorUsuario(int usuarioId);

        // USUARIOS
        Task<List<UsuarioDTO>> ListarUsuarios();
        Task<UsuarioDTO> ObtenerUsuarioPorId(int id);
        Task<int> CrearUsuario(UsuarioCreateRequest request);
        Task ActualizarUsuario(UsuarioUpdateRequest request);
        Task CambiarEstadoUsuario(int id, int estado);

        // MENUS
        Task<List<MenuDTO>> ListarMenus();
        Task<MenuDTO> ObtenerMenuPorId(int id);

        // ROLES
        Task<List<Rol>> ListarRoles();

        // DASHBOARD IMAGES
        Task<List<ImagenDashboard>> ListarImagenesDashboard();
        Task RegistrarImagenDashboard(string url, int correlativo);
        Task CambiarEstadoImagenDashboard(int id, int estado);
    }
}
