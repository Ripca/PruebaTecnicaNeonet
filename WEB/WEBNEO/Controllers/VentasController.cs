using Microsoft.AspNetCore.Mvc;
using WEBNEO.Services;
using WEBNEO.Entities;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace WEBNEO.Controllers
{
    public class VentasController : BaseController
    {
        private readonly IApiService _apiService;

        public VentasController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var ventas = await _apiService.ListarVentas();
            return View(ventas);
        }

        public async Task<IActionResult> Nueva()
        {
            // We need clients and products to populate dropdowns/selection
            ViewBag.Clientes = await _apiService.ListarClientes();
            ViewBag.Productos = await _apiService.ListarProductos();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] VentaRequest request)
        {
             var result = await _apiService.RegistrarVenta(request);
             return Json(result);
        }



        public async Task<IActionResult> Detalle(int id)
        {
            var detalle = await _apiService.ObtenerDetalleVenta(id);
            return Json(detalle);
        }

        public IActionResult VentasCliente()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetHistorialJson(int clienteId)
        {
            var historial = await _apiService.ObtenerVentasPorCliente(clienteId);
            return Json(historial);
        }
    }
}
