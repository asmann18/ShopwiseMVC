using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.Areas.Admin.ViewModels.CategoryVMs;
using Shopwise.DAL;
using Shopwise.Entities;
using Shopwise.Helpers;

namespace Shopwise.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;



        public CategoryController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            bool isExist = await _context.Categories.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda category movcuddur");
                return View(vm);
            }
            string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
            Category category = new()
            {
                Name = vm.Name,
                ImagePath = await vm.Image.GeneratePhoto(path)

            };


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

            CategoryUpdateVM vm = new() { Name = category.Name, Id = category.Id };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateVM category)
        {

            Category existed = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existed is null)
                return NotFound();
            bool isExist = await _context.Categories.AnyAsync(x => x.Name.ToLower() == category.Name.ToLower() && x.Id != category.Id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda category movcuddur");
                return View(category);
            }
            existed.Name = category.Name;
            if (category.Image is not null)
            {
                string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
                existed.ImagePath.DeleteImage(path);
                existed.ImagePath = await category.Image.GeneratePhoto(path);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
