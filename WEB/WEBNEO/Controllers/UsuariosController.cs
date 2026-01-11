using Microsoft.AspNetCore.Mvc;
using WEBNEO.Services;
using WEBNEO.Entities;
using System.Text;

namespace WEBNEO.Controllers
{
    public class UsuariosController : BaseController
    {
        private readonly IApiService _apiService;

        public UsuariosController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // Doble Base64 Encoding/Decoding
        private string EncodeDoubleBase64(int id)
        {
            var first = Convert.ToBase64String(Encoding.UTF8.GetBytes(id.ToString()));
            var second = Convert.ToBase64String(Encoding.UTF8.GetBytes(first));
            return Uri.EscapeDataString(second);
        }

        private int DecodeDoubleBase64(string encoded)
        {
            var decoded = Uri.UnescapeDataString(encoded);
            var first = Encoding.UTF8.GetString(Convert.FromBase64String(decoded));
            var second = Encoding.UTF8.GetString(Convert.FromBase64String(first));
            return int.Parse(second);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _apiService.ListarUsuarios();
                return View(usuarios);
            }
            catch
            {
                return View(new List<UsuarioDTO>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(string id = null)
        {
            ViewBag.Roles = await _apiService.ListarRoles();
            
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var userId = DecodeDoubleBase64(id);
                    var usuario = await _apiService.ObtenerUsuarioPorId(userId);
                    // Pass the first selected role to the view for the single-select dropdown
                    ViewBag.SelectedRolId = usuario.RolesIdsList.FirstOrDefault();
                    return View(usuario);
                }
                catch
                {
                    return View();
                }
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioDTO model, string id = null)
        {
            try
            {
                // Validación de Email (Regex)
                var emailRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                if (!emailRegex.IsMatch(model.Email))
                {
                    ModelState.AddModelError("Email", "El formato del correo electrónico no es válido.");
                    throw new Exception("El formato del correo electrónico no es válido.");
                }

                if (string.IsNullOrEmpty(id))
                {
                    // Crear nuevo
                    var request = new UsuarioCreateRequest
                    {
                        RolesIds = new List<int> { int.Parse(Request.Form["RolId"]) },
                        Nombre = model.Nombre,
                        Email = model.Email,
                        Password = Request.Form["Password"]
                    };
                    var result = await _apiService.CrearUsuario(request);
                    TempData["Message"] = result.Message;
                }
                else
                {
                    // Actualizar
                    var userId = DecodeDoubleBase64(id);
                    var request = new UsuarioUpdateRequest
                    {
                        Id = userId,
                        RolesIds = new List<int> { int.Parse(Request.Form["RolId"]) },
                        Nombre = model.Nombre,
                        Email = model.Email,
                        Password = Request.Form["Password"]
                    };
                    var result = await _apiService.ActualizarUsuario(request);
                    TempData["Message"] = result.Message;
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = await _apiService.ListarRoles();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, int estado)
        {
            var result = await _apiService.CambiarEstadoUsuario(id, estado);
            return Json(result);
        }
    }
}
