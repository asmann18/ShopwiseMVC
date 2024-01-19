using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.DAL;
using Shopwise.Entities;

namespace Shopwise.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var settings = _context.Settings.ToList();
            return View(settings);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }

            bool isExist = await _context.Settings.AnyAsync(x => x.Key.ToLower() == setting.Key.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Key", "Bu key movcuddur");
                return View(setting);
            }

            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (setting is null) { return NotFound(); }

            return View(setting);

        }

        [HttpPost]
        public async Task<IActionResult> Update(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }
            var existed = await _context.Settings.FirstOrDefaultAsync(x => x.Id == setting.Id);
            if (existed is null)
                return BadRequest();
            bool isExist = await _context.Settings.AnyAsync(x => x.Key.ToLower() == setting.Key.ToLower() && x.Id != setting.Id);
            if (isExist)
            {
                ModelState.AddModelError("Key", "Bu key movcuddur");
                return View(setting);
            }

            existed.Value = setting.Value;
            existed.Key = setting.Key;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (setting is null) { return NotFound(); }

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
