using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.Areas.Admin.ViewModels;
using Shopwise.Areas.Admin.ViewModels.ProductVMs;
using Shopwise.DAL;
using Shopwise.Entities;
using Shopwise.Helpers;

namespace Shopwise.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ProductController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public IActionResult Index()
    {
        var products = _context.Products.Include(x => x.ProductImages).ToList();
        return View(products);
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Tags = await _context.Tags.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Tags = await _context.Tags.ToListAsync();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        bool existCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
        if (!existCategory)
        {
            ModelState.AddModelError("CategoryId", "Bele category movcud deyildir");
            return View(vm);
        }
        foreach (var item in vm.TagIds)
        {
            var isExist = await _context.Tags.AnyAsync(x => x.Id == item);
            if (!isExist)
            {

                ModelState.AddModelError("TagIds", "Bele category movcud deyildir");
                return View(vm);
            }
        }


        foreach (IFormFile image in vm.ProductImages)
        {
            if (!image.ValidateType())
            {
                ModelState.AddModelError("Image", "Please enter image");
                return View(vm);
            }
            if (!image.ValidateSize(2))
            {
                ModelState.AddModelError("Image", "Image size max size is 2mb");
                return View(vm);

            }
        }
        Product product = new()
        {
            Name = vm.Name,
            Price = vm.Price,
            ReturnPolicy = vm.ReturnPolicy,
            ShortDescription = vm.ShortDescription,
            SKU = vm.SKU,
            TwitterLink = vm.TwitterLink,
            Warranty = vm.Warranty,
            Offer = vm.Offer,
            LongDescription = vm.LongDescription,
            FacebookLink = vm.FacebookLink,
            CategoryId = vm.CategoryId,
            CashOnDelivery = vm.CashOnDelivery,
            ProductImages = new(),
            ProduductTags=new()

        };
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
        foreach (IFormFile image in vm.ProductImages)
        {
            ProductImage img = new()
            {
                Product = product,
                Path = await image.GeneratePhoto(path),

            };
            product.ProductImages.Add(img);
        }
        foreach (var id in vm.TagIds)
        {
            ProductTag tag = new()
            {
                Product = product,
                Tag=_context.Tags.FirstOrDefault(x=>x.Id==id),

            };
            product.ProduductTags.Add(tag);
        }
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");


    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);
        if (product is null)
        {
            return NotFound();
        }
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
        foreach (var img in product.ProductImages)
        {
            img.Path.DeleteImage(path);
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();

        ViewBag.Categories = await _context.Categories.ToListAsync();

        ProductUpdateVM vm = new()
        {
            Id = id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            FacebookLink = product.FacebookLink,
            ReturnPolicy = product.ReturnPolicy,
            CashOnDelivery = product.CashOnDelivery,
            LongDescription = product.LongDescription,
            Offer = product.Offer,
            Price = product.Price,
            ShortDescription = product.ShortDescription,
            SKU = product.SKU,
            TwitterLink = product.TwitterLink,
            Warranty = product.Warranty,
        };
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateVM vm)
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();

        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var existedProduct = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (existedProduct is null)
            return NotFound();

        bool existCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
        if (!existCategory)
        {
            ModelState.AddModelError("CategoryId", "Bele category movcud deyildir");
            return View(vm);
        }
        if (vm.ProductImages is not null)
        {

            foreach (IFormFile image in vm.ProductImages)
            {
                if (!image.ValidateType())
                {
                    ModelState.AddModelError("Image", "Please enter image");
                    return View(vm);
                }
                if (!image.ValidateSize(2))
                {
                    ModelState.AddModelError("Image", "Image size max size is 2mb");
                    return View(vm);
                }
            }

        }


        existedProduct.Name = vm.Name;
        existedProduct.Price = vm.Price;
        existedProduct.ReturnPolicy = vm.ReturnPolicy;
        existedProduct.ShortDescription = vm.ShortDescription;
        existedProduct.SKU = vm.SKU;
        existedProduct.TwitterLink = vm.TwitterLink;
        existedProduct.Warranty = vm.Warranty;
        existedProduct.Offer = vm.Offer;
        existedProduct.LongDescription = vm.LongDescription;
        existedProduct.FacebookLink = vm.FacebookLink;
        existedProduct.CategoryId = vm.CategoryId;
        existedProduct.CashOnDelivery = vm.CashOnDelivery;


        if (vm.ProductImages != null)
        {
            string path = Path.Combine(_environment.ContentRootPath, "wwwroot", "assets", "images");
            foreach (var img in existedProduct.ProductImages)
            {
                img.Path.DeleteImage(path);
            }

            _context.ProductImages.RemoveRange(existedProduct.ProductImages);
            existedProduct.ProductImages = new();

            foreach (IFormFile image in vm.ProductImages)
            {
                ProductImage img = new()
                {
                    Product = existedProduct,
                    Path = await image.GeneratePhoto(path),

                };
                existedProduct.ProductImages.Add(img);
            }
        }

        _context.Products.Update(existedProduct);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");

    }


    public async Task<IActionResult> Detail(int id)
    {
        var product = await _context.Products.Include(x => x.ProductImages).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (product is null)
            return NotFound();
        return View(product);
    }
}
