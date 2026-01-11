using Microsoft.AspNetCore.Mvc;
using WEBNEO.Services;
using System.Threading.Tasks;

namespace WEBNEO.Controllers
{
    public class ClientesController : BaseController
    {
        private readonly IApiService _apiService;

        public ClientesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _apiService.ListarClientes();
            return View(clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var clientes = await _apiService.ListarClientes();
            return Json(new { data = clientes });
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string email)
        {
            var cliente = await _apiService.BuscarClientePorEmail(email);
            return Json(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] WEBNEO.Entities.Cliente request)
        {
            var result = await _apiService.CrearCliente(request);
            return Json(result);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] WEBNEO.Entities.Cliente request)
        {
            var result = await _apiService.ActualizarCliente(request);
            return Json(result);
        }

        [HttpPatch]
        public async Task<IActionResult> CambiarEstado(int id, int estado)
        {
            var result = await _apiService.CambiarEstadoCliente(id, estado);
            return Json(result);
        }
    }
}
