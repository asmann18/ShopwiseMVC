using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.Areas.Admin.ViewModels.ProductVMs;
using Shopwise.Areas.Admin.ViewModels.ServiceVMs;
using Shopwise.Areas.Admin.ViewModels.SliderVMs;
using Shopwise.DAL;
using Shopwise.Entities;
using Shopwise.Helpers;

namespace Shopwise.Areas.Admin.Controllers;

[Area("Admin")]
public class ServiceController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    public ServiceController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public IActionResult Index()
    {
        var services = _context.Services.ToList();
        return View(services);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ServiceCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
        Service service = new()
        {
            Name = vm.Name,
            Description = vm.Description,
            ImagePath = await vm.Image.GeneratePhoto(path)

        };



        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Delete(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
        if (service is null)
            return NotFound();

        _context.Services.Remove(service);
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
        service.ImagePath.DeleteImage(path);

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
        if (service is null)
            return NotFound();
        ServiceUpdateVM vm = new()
        {
            Id=id,
            Name=service.Name,
            Description=service.Description,
        };
        return View(vm);

    }
    [HttpPost]
    public async Task<IActionResult> Update(ServiceUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var exist = await _context.Services.FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (exist is null)
        {
            return NotFound();
        }
        exist.Name = vm.Name;
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
