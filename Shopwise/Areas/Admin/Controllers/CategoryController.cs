using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.DAL;
using Shopwise.Entities;

namespace Shopwise.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            bool isExist = await _context.Categories.AnyAsync(x => x.Name.ToLower() == category.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda category movcuddur");
                return View(category);
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
                return NotFound();

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {

            Category existed = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existed is null)
                return NotFound();
            bool isExist=await _context.Categories.AnyAsync(x=>x.Name.ToLower()==category.Name.ToLower() && x.Id!=category.Id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda category movcuddur");
                return View(category);
            }
            existed.Name=category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
