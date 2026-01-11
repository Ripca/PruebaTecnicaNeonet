using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APINEO.BLL;
using APINEO.Entities.Models;

namespace APINEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public UsuariosController(IEmpresaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                var usuarios = await _service.ListarUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al listar usuarios", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            try
            {
                var usuario = await _service.ObtenerUsuarioPorId(id);
                if (usuario == null)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener usuario", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioCreateRequest request)
        {
            try
            {
                // Validación de Email
                if (!System.Text.RegularExpressions.Regex.IsMatch(request.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return BadRequest(new { message = "El formato del correo electrónico no es válido." });
                }

                var nuevoId = await _service.CrearUsuario(request);
                
                if (nuevoId == -1)
                    return BadRequest(new { message = "El email ya está registrado" });

                return Ok(new { id = nuevoId, message = "Usuario creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear usuario", error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarUsuario([FromBody] UsuarioUpdateRequest request)
        {
            try
            {
                // Validación de Email
                if (!System.Text.RegularExpressions.Regex.IsMatch(request.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return BadRequest(new { message = "El formato del correo electrónico no es válido." });
                }

                await _service.ActualizarUsuario(request);
                return Ok(new { message = "Usuario actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar usuario", error = ex.Message });
            }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoUsuario(int id, [FromBody] int estado)
        {
            try
            {
                await _service.CambiarEstadoUsuario(id, estado);
                return Ok(new { message = "Estado actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al cambiar estado", error = ex.Message });
            }
        }
    }
}
