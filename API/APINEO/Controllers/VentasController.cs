using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APINEO.BLL;
using APINEO.Entities.Models;
using System;
using System.Threading.Tasks;

namespace APINEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public VentasController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var result = await _service.ListarVentas();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VentaRequest request)
        {
            try
            {
                await _service.RegistrarVenta(request);
                return Ok(new { mensaje = "Venta registrada con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }



        [HttpGet("por-cliente/{clienteId}")]
        public async Task<IActionResult> ObtenerVentasPorCliente(int clienteId)
        {
            try
            {
                var result = await _service.ObtenerVentasPorCliente(clienteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error en API: " + ex.Message });
            }
        }

        [HttpGet("detalle/{ventaId}")]
        public async Task<IActionResult> Detalle(int ventaId)
        {
            var result = await _service.ObtenerDetalleVenta(ventaId);
            return Ok(result);
        }
    }
}
