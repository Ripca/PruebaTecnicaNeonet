using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APINEO.BLL;
using APINEO.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APINEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public ClientesController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var result = await _service.ListarClientes();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var result = await _service.ObtenerClientePorId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("buscar/{email}")]
        public async Task<IActionResult> BuscarPorEmail(string email)
        {
            var result = await _service.ObtenerClientePorEmail(email);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] Cliente request)
        {
            var id = await _service.CrearCliente(request.Nombre, request.Email);
            return Ok(new { id });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Cliente request)
        {
            await _service.ActualizarCliente(request.Id, request.Nombre, request.Email);
            return Ok();
        }

        [HttpPatch("{id}/estado/{estado}")]
        public async Task<IActionResult> CambiarEstado(int id, int estado)
        {
            await _service.CambiarEstadoCliente(id, estado);
            return Ok();
        }

        [HttpGet("total/vendido-por-cliente")]
        public async Task<IActionResult> ReporteVentas()
        {
            var result = await _service.ObtenerReporteTotalVendidoCliente();
            return Ok(result);
        }
    }
}
