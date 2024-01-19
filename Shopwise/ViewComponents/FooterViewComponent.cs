using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopwise.DAL;
using Shopwise.Entities;
using Shopwise.ViewModels;

namespace Shopwise.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            List<Service> services = await _context.Services.ToListAsync();
            FooterVM vm = new() { Services = services,Settings=settings };
            return View(vm);
        }
    }
}
