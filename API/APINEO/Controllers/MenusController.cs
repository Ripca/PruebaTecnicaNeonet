using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APINEO.BLL;

namespace APINEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenusController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public MenusController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ListarMenus()
        {
            try
            {
                var menus = await _service.ListarMenus();
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al listar menús", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerMenuPorId(int id)
        {
            try
            {
                var menu = await _service.ObtenerMenuPorId(id);
                if (menu == null)
                    return NotFound(new { message = "Menú no encontrado" });

                return Ok(menu);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener menú", error = ex.Message });
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerMenusPorUsuario(int usuarioId)
        {
            try
            {
                var menus = await _service.ObtenerMenusPorUsuario(usuarioId);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener menús del usuario", error = ex.Message });
            }
        }
    }
}
