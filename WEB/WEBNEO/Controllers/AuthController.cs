using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WEBNEO.Services;
using WEBNEO.Entities;

namespace WEBNEO.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya está autenticado, redirigir al home
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                // Capturar token de reCAPTCHA del formulario
                request.TokenCaptcha = HttpContext.Request.Form["g-recaptcha-response"];

                var response = await _apiService.Login(request);
                
                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    // Guardar en sesión
                    HttpContext.Session.SetString("JwtToken", response.Token);
                    HttpContext.Session.SetInt32("UserId", response.Id);
                    HttpContext.Session.SetString("UserName", response.Nombre);
                    HttpContext.Session.SetString("UserEmail", response.Email);
                    HttpContext.Session.SetString("UserRoles", string.Join(",", response.RolesIds));

                    // Obtener menús del usuario y guardarlos en sesión
                    var menus = await _apiService.ObtenerMenusPorUsuario(response.Id);
                    var menusJson = System.Text.Json.JsonSerializer.Serialize(menus);
                    HttpContext.Session.SetString("UserMenus", menusJson);

                    return RedirectToAction("Index", "Home");
                }
                
                ViewBag.Error = "Credenciales inválidas";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al iniciar sesión: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
