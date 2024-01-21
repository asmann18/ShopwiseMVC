using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.Areas.Admin.ViewModels.SliderVMs;
using Shopwise.DAL;
using Shopwise.Entities;
using Shopwise.Helpers;

namespace Shopwise.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]

public class SliderController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    public SliderController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
    public IActionResult Index()
    {
        var sliders = _context.Sliders.AsEnumerable();
        return View(sliders);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SliderCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
        Slider slider = new()
        {
            Title = vm.Title,
            Description = vm.Description,
            ImagePath = await vm.Image.GeneratePhoto(path)

        };



        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int id)
    {
        Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
        if (slider == null)
        {
            return NotFound();
        }
        _context.Sliders.Remove(slider);
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
        slider.ImagePath.DeleteImage(path);

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int id)
    {
        Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
        if (slider is null)
            return NotFound();

        SliderUpdateVM vm = new()
        {
            Id = slider.Id,
            Title = slider.Title,
            Description = slider.Description,
        };
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(SliderUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        Slider exist = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (exist is null)
        {
            return NotFound();
        }
        exist.Title = vm.Title;
        exist.Description = vm.Description;
        if (vm.Image is not null)
        {
            string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
            exist.ImagePath.DeleteImage(path);
            exist.ImagePath = await vm.Image.GeneratePhoto(path);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

}
