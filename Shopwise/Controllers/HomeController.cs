using Microsoft.AspNetCore.Mvc;

namespace Shopwise.Controllers
{
    public class HomeController : Controller

    {
        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
