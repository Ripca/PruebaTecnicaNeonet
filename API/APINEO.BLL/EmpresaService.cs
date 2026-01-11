using APINEO.Entities.Models;
using APINEO.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APINEO.BLL
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _repository;

        public EmpresaService(IEmpresaRepository repository)
        {
            _repository = repository;
        }

        // PRODUCTOS
        public async Task<List<Producto>> ListarProductos()
        {
            return await _repository.ListarProductos();
        }

        public async Task<Producto> ObtenerProductoPorId(int id)
        {
            return await _repository.ObtenerProducto(id);
        }

        public async Task<int> CrearProducto(string nombre, double? precio, int? stock)
        {
            return await _repository.CrearProducto(nombre, precio, stock);
        }

        public async Task ActualizarProducto(int id, string nombre, double? precio, int? stock)
        {
            await _repository.ActualizarProducto(id, nombre, precio, stock);
        }

        public async Task CambiarEstadoProducto(int id, int estado)
        {
            await _repository.CambiarEstadoProducto(id, estado);
        }

        // CLIENTES
        public async Task<List<Cliente>> ListarClientes()
        {
            return await _repository.ListarClientes();
        }

        public async Task<Cliente> ObtenerClientePorId(int id)
        {
            return await _repository.ObtenerClientePorId(id);
        }

        public async Task<Cliente> ObtenerClientePorEmail(string email)
        {
            return await _repository.ObtenerClientePorEmail(email);
        }

        public async Task<int> CrearCliente(string nombre, string email)
        {
            return await _repository.CrearCliente(nombre, email);
        }

        public async Task ActualizarCliente(int id, string nombre, string email)
        {
            await _repository.ActualizarCliente(id, nombre, email);
        }

        public async Task CambiarEstadoCliente(int id, int estado)
        {
            await _repository.CambiarEstadoCliente(id, estado);
        }

        // REPORTES
        public async Task<List<ReporteProducto>> ObtenerReporteProductosMasVendidos()
        {
            return await _repository.ObtenerReporteProductosMasVendidos();
        }



        public async Task<List<ReporteCliente>> ObtenerReporteTotalVendidoCliente()
        {
            return await _repository.ObtenerReporteTotalVendidoCliente();
        }

        // VENTAS
        public async Task RegistrarVenta(VentaRequest request)
        {
            //Valida el Stock
            foreach (var det in request.Detalles)
            {
                var prod = await _repository.ObtenerProducto(det.ProductoId);
                if (prod == null) throw new Exception($"Producto {det.ProductoId} no existe.");
                if (prod.Stock < det.Cantidad) throw new Exception($"Stock insuficiente para {prod.Nombre}. Stock actual: {prod.Stock}");
            }

            //Crear Venta 
            var nuevaVenta = new Venta
            {
                ClienteId = request.ClienteId,
                Fecha = DateOnly.FromDateTime(DateTime.Now)
            };

            int ventaId = await _repository.RegistrarVenta(nuevaVenta);

            // Crea Detalles de la venta
            foreach (var det in request.Detalles)
            {
                var detalle = new DetalleVenta
                {
                    VentaId = ventaId,
                    ProductoId = det.ProductoId,
                    Cantidad = det.Cantidad,
                    PrecioUnitario = det.PrecioUnitario
                };
                await _repository.RegistrarDetalleVenta(detalle);
            }
        }

        public async Task<List<VentaDTO>> ListarVentas()
        {
            return await _repository.ListarVentas();
        }



        public async Task<List<VentaDTO>> ObtenerVentasPorCliente(int clienteId)
        {
            return await _repository.ObtenerVentasPorCliente(clienteId);
        }

        public async Task<List<VentaHistorialDTO>> ObtenerDetalleVenta(int ventaId)
        {
            return await _repository.ObtenerDetalleVenta(ventaId);
        }

        public async Task<List<object>> ListarTodasLasVentas()
        {
            return await _repository.ListarTodasLasVentas();
        }

        // AUTENTICACIÓN
        public async Task<UsuarioDTO> ValidarUsuario(string email, string password)
        {
            return await _repository.ValidarUsuario(email, password);
        }

        public async Task<List<MenuDTO>> ObtenerMenusPorUsuario(int usuarioId)
        {
            var menusPlanos = await _repository.ObtenerMenusPorUsuario(usuarioId);
            return ConstruirArbolMenus(menusPlanos);
        }

        // USUARIOS
        public async Task<List<UsuarioDTO>> ListarUsuarios()
        {
            return await _repository.ListarUsuarios();
        }

        public async Task<UsuarioDTO> ObtenerUsuarioPorId(int id)
        {
            return await _repository.ObtenerUsuarioPorId(id);
        }

        public async Task<int> CrearUsuario(UsuarioCreateRequest request)
        {
            return await _repository.CrearUsuario(request);
        }

        public async Task ActualizarUsuario(UsuarioUpdateRequest request)
        {
            await _repository.ActualizarUsuario(request);
        }

        public async Task CambiarEstadoUsuario(int id, int estado)
        {
            await _repository.CambiarEstadoUsuario(id, estado);
        }

        // MENUS
        public async Task<List<MenuDTO>> ListarMenus()
        {
            var menusPlanos = await _repository.ListarMenus();
            return ConstruirArbolMenus(menusPlanos);
        }

        public async Task<MenuDTO> ObtenerMenuPorId(int id)
        {
            return await _repository.ObtenerMenuPorId(id);
        }

        // ROLES
        public async Task<List<Rol>> ListarRoles()
        {
            return await _repository.ListarRoles();
        }

        // DASHBOARD IMAGES
        public async Task<List<ImagenDashboard>> ListarImagenesDashboard()
        {
            return await _repository.ListarImagenesDashboard();
        }

        public async Task RegistrarImagenDashboard(string url, int correlativo)
        {
            await _repository.RegistrarImagenDashboard(url, correlativo);
        }

        public async Task CambiarEstadoImagenDashboard(int id, int estado)
        {
            await _repository.CambiarEstadoImagenDashboard(id, estado);
        }

        // Construir menu
        private List<MenuDTO> ConstruirArbolMenus(List<MenuDTO> menusPlanos)
        {
            var menusPadre = menusPlanos.Where(m => m.MenuPadreId == null).ToList();

            foreach (var padre in menusPadre)
            {
                padre.SubMenus = menusPlanos
                    .Where(m => m.MenuPadreId == padre.Id)
                    .OrderBy(m => m.Orden)
                    .ToList();
            }

            return menusPadre.OrderBy(m => m.Orden).ToList();
        }
    }
}
