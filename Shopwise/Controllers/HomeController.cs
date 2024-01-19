using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                Products = _context.Products.ToList(),
                SpecialOffer = _context.SpecialOffers.FirstOrDefault(),
                TopCategories = _context.Categories.ToList(),
                TrendingProducts = _context.Products.Include(x=>x.ProductImages).Include(x=>x.Category).Where(x=>x.Category.Name=="Trending").ToList(),
                OfferSections = _context.OfferSections.ToList(),

            };
            return View(vm);
        }
    }
}
