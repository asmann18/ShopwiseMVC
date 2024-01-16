namespace Shopwise.Areas.Admin.ViewModels.ServiceVMs
{
    public class ServiceUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
