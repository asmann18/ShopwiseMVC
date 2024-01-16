using Microsoft.AspNetCore.Mvc;
using Shopwise.DAL;
using Shopwise.ViewModels;

namespace Shopwise.Controllers
{
    public class HomeController : Controller

    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        

        public async Task<IActionResult> Index()
        {

            HomeVM vm = new()
            {
                Sliders = _context.Sliders.ToList(),
            };
            return View(vm);
        }
    }
}
