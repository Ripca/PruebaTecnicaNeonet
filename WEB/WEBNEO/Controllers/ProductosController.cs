using Microsoft.AspNetCore.Mvc;
using WEBNEO.Services;
using System.Threading.Tasks;

namespace WEBNEO.Controllers
{
    public class ProductosController : BaseController
    {
        private readonly IApiService _apiService;

        public ProductosController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _apiService.ListarProductos();
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var productos = await _apiService.ListarProductos();
            return Json(new { data = productos });
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var producto = await _apiService.ObtenerProductoPorId(id);
            return Json(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] WEBNEO.Entities.Producto request)
        {
            var result = await _apiService.CrearProducto(request);
            return Json(result);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] WEBNEO.Entities.Producto request)
        {
            var result = await _apiService.ActualizarProducto(request);
            return Json(result);
        }

        [HttpPatch]
        public async Task<IActionResult> CambiarEstado(int id, int estado)
        {
            var result = await _apiService.CambiarEstadoProducto(id, estado);
            return Json(result);
        }
    }
}
