namespace Shopwise.Areas.Admin.ViewModels.CategoryVMs;

public class CategoryUpdateVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
}
