using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.Areas.Admin.ViewModels.OfferSectionVMs;
using Shopwise.Areas.Admin.ViewModels.SliderVMs;
using Shopwise.DAL;
using Shopwise.Entities;
using Shopwise.Helpers;

namespace Shopwise.Areas.Admin.Controllers;

[Area("Admin")]
public class OfferSectionController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;



    public OfferSectionController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public IActionResult Index()
    {
        var offerSections = _context.OfferSections.ToList();
        return View(offerSections);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(OfferCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
        OfferSection offerSection = new()
        {
            Title = vm.Title,
            Description = vm.Description,
            ImagePath = await vm.Image.GeneratePhoto(path)

        };



        await _context.OfferSections.AddAsync(offerSection);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int id)
    {
        var offer = await _context.OfferSections.FirstOrDefaultAsync(x => x.Id == id);
        if (offer == null)
        {
            return NotFound();
        }
        _context.OfferSections.Remove(offer);
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
        offer.ImagePath.DeleteImage(path);

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var offer = await _context.OfferSections.FirstOrDefaultAsync(x => x.Id == id);
        if (offer is null)
            return NotFound();

        OfferUpdateVM vm = new()
        {
            Id = offer.Id,
            Title = offer.Title,
            Description = offer.Description,
        };
        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(OfferUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var exist = await _context.OfferSections.FirstOrDefaultAsync(x => x.Id == vm.Id);
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
