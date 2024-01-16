namespace Shopwise.Areas.Admin.ViewModels.ServiceVMs;

public class ServiceCreateVM
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
}
