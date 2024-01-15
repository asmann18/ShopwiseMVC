using Microsoft.EntityFrameworkCore;
using Shopwise.Entities;

namespace Shopwise.DAL;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }


    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Categories  { get; set; }
}
