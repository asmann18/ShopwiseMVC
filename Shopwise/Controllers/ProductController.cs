using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.DAL;
using Shopwise.Entities;

namespace Shopwise.Controllers
{
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        public ProductController(AppDbContext context,UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageCount = (int)Math.Ceiling((decimal)_context.Products.Count() / (decimal)10);
            ViewBag.PageCount=pageCount;
            if (page > pageCount)
                page = pageCount;
            ViewBag.CurrentPage=page;
            
            var products=await _context.Products.Include(x=>x.ProductImages).Skip((page-1)*10).Take(10).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.Include(x => x.ProductImages).Include(x => x.ProduductTags).ThenInclude(x => x.Tag).Include(x => x.Category).SingleOrDefaultAsync(p => p.Id == id);
            return View(product);
        }
        [Authorize]
        public async Task<IActionResult> AddWishlist(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
                return NotFound();
            var user =await _userManager.FindByNameAsync(User.Identity.Name);


            var exist=await _context.WishLists.FirstOrDefaultAsync(x=>x.CustomUserId == user.Id && x.ProductId==product.Id);
            if (exist is not null)
            {
                _context.WishLists.Remove(exist);
                await _context.SaveChangesAsync();
                return RedirectToAction("Detail", new { id = id });
            }

            WishList wishList = new()
            {
                Product = product,
                CustomUser =user
            };
            await _context.WishLists.AddAsync(wishList);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = id });
        }

    }
}
