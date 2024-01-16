using Microsoft.EntityFrameworkCore;
using Shopwise.Entities;

namespace Shopwise.DAL;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }


    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Category> Categories  { get; set; }
    public DbSet<Product> Products   { get; set; }
    public DbSet<ProductImage> ProductImages   { get; set; }
    public DbSet<OfferSection> OfferSections{ get; set; }
}
