using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.DAL;

namespace Shopwise.Controllers
{
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product =await _context.Products.Include(x=>x.ProductImages).Include(x=>x.ProduductTags).ThenInclude(x=>x.Tag).Include(x=>x.Category).SingleOrDefaultAsync(p => p.Id == id);
            return View(product);
        }
        
    }
}
