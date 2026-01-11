using System.Collections.Generic;
using System.Threading.Tasks;
using WEBNEO.Entities;

namespace WEBNEO.Services
{
    public interface IApiService
    {
        // AUTENTICACIÓN
        Task<LoginResponse> Login(LoginRequest request);
        Task<List<MenuDTO>> ObtenerMenusPorUsuario(int usuarioId);

        // PRODUCTOS
        Task<List<Producto>> ListarProductos();
        Task<Producto> ObtenerProductoPorId(int id);
        Task<ResponseResult> CrearProducto(Producto request);
        Task<ResponseResult> ActualizarProducto(Producto request);
        Task<ResponseResult> CambiarEstadoProducto(int id, int estado);

        // CLIENTES
        Task<List<Cliente>> ListarClientes();
        Task<Cliente> ObtenerClientePorId(int id);
        Task<Cliente> BuscarClientePorEmail(string email);
        Task<ResponseResult> CrearCliente(Cliente request);
        Task<ResponseResult> ActualizarCliente(Cliente request);
        Task<ResponseResult> CambiarEstadoCliente(int id, int estado);

        // VENTAS
        Task<ResponseResult> RegistrarVenta(VentaRequest request);
        Task<List<VentaDTO>> ListarVentas();

        Task<List<VentaDTO>> ObtenerVentasPorCliente(int clienteId);
        Task<List<VentaHistorialDTO>> ObtenerDetalleVenta(int ventaId);



        // USUARIOS
        Task<List<UsuarioDTO>> ListarUsuarios();
        Task<UsuarioDTO> ObtenerUsuarioPorId(int id);
        Task<ResponseResult> CrearUsuario(UsuarioCreateRequest request);
        Task<ResponseResult> ActualizarUsuario(UsuarioUpdateRequest request);
        Task<ResponseResult> CambiarEstadoUsuario(int id, int estado);

        // ROLES
        Task<List<Rol>> ListarRoles();

        // DASHBOARD
        Task<List<ImagenDashboard>> ListarImagenesDashboard();

    }
}
