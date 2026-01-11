using Microsoft.AspNetCore.Mvc;
using WEBNEO.Services;

namespace WEBNEO.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IApiService _apiService;

        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var imagenes = await _apiService.ListarImagenesDashboard();
            return View(imagenes);
        }
    }
}
