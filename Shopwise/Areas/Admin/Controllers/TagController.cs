using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.DAL;
using Shopwise.Entities;

namespace Shopwise.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {

        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tags = _context.Tags.ToList();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            bool isExist = await _context.Tags.AnyAsync(x => x.Name.ToLower() == tag.Name.ToLower());

            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu addda Tag movcuddur");
                return View(tag);
            }

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {

            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            var existed = await _context.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id);
            if (existed == null)
            {
                return NotFound();
            }

            bool isExist = await _context.Tags.AnyAsync(x => x.Name.ToLower() == tag.Name.ToLower() && x.Id != tag.Id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda tag movcuddur");
                return View(tag);
            }
            existed.Name = tag.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(y => y.Id == id);
            if (tag is null)
                return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
