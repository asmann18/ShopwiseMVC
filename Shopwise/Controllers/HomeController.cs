using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.DAL;
using Shopwise.Entities;
using Shopwise.ViewModels;

namespace Shopwise.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        public HomeController(AppDbContext context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        [Authorize]
        public IActionResult Wishlist()
        {
            var wishlist = _context.WishLists.Include(x=>x.CustomUser).Include(x=>x.Product).ThenInclude(x=>x.ProductImages).Where(x => x.CustomUser.UserName == User.Identity.Name).ToList();
            if (wishlist is null) wishlist = new();
            return View(wishlist);
        }

        public async Task<IActionResult> RemoveWishlist(int id)
        {
            var item=await _context.WishLists.FirstOrDefaultAsync(x=>x.Id==id);
            if (item is null)
                return NotFound();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (item.CustomUserId!=user.Id)
            {
                return BadRequest();
            }



            _context.WishLists.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Wishlist");
        }
    }
}
