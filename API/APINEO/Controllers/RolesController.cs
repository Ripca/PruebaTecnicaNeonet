using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APINEO.BLL;

namespace APINEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public RolesController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ListarRoles()
        {
            try
            {
                var roles = await _service.ListarRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al listar roles", error = ex.Message });
            }
        }
    }
}
