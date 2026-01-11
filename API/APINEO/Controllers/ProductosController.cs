using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Collections.Generic;
using System.Threading.Tasks;
using APINEO.BLL;
using APINEO.Entities.Models;

namespace APINEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public ProductosController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var result = await _service.ListarProductos();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var result = await _service.ObtenerProductoPorId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] Producto request)
        {
            var id = await _service.CrearProducto(request.Nombre, request.Precio, request.Stock);
            return Ok(new { id });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Producto request)
        {
            await _service.ActualizarProducto(request.Id, request.Nombre, request.Precio, request.Stock);
            return Ok();
        }

        [HttpPatch("{id}/estado/{estado}")]
        public async Task<IActionResult> CambiarEstado(int id, int estado)
        {
            await _service.CambiarEstadoProducto(id, estado);
            return Ok();
        }

        [HttpGet("mas-vendidos")]
        public async Task<IActionResult> MasVendidos()
        {
            var result = await _service.ObtenerReporteProductosMasVendidos();
            return Ok(result);
        }


    }
}
