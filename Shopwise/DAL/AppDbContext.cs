using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopwise.Entities;

namespace Shopwise.DAL;

public class AppDbContext:IdentityDbContext<CustomUser>
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
    public DbSet<OfferSection> SpecialOffers{ get; set; }
    public DbSet<Setting> Settings{ get; set; }
    public DbSet<Tag> Tags{ get; set; }
    public DbSet<ProductTag> ProductTags{ get; set; }
    public DbSet<WishList> WishLists{ get; set; }

}
