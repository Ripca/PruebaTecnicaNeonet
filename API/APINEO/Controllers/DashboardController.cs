using Microsoft.AspNetCore.Mvc;
using APINEO.BLL;
using APINEO.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APINEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public DashboardController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet("Imagenes")]
        public async Task<ActionResult<List<ImagenDashboard>>> ListarImagenes()
        {
            var result = await _service.ListarImagenesDashboard();
            return Ok(result);
        }

        [HttpPost("RegistrarImagen")]
        public async Task<IActionResult> RegistrarImagen([FromBody] ImagenDashboard request)
        {
            await _service.RegistrarImagenDashboard(request.Url, request.Correlativo ?? 0);
            return Ok(new { success = true, message = "Imagen registrada" });
        }

        [HttpPatch("CambiarEstadoImagen/{id}/{estado}")]
        public async Task<IActionResult> CambiarEstadoImagen(int id, int estado)
        {
            await _service.CambiarEstadoImagenDashboard(id, estado);
            return Ok(new { success = true, message = "Estado actualizado" });
        }
    }
}
