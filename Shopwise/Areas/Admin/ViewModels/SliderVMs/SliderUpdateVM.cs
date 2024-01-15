namespace Shopwise.Areas.Admin.ViewModels.SliderVMs
{
    public class SliderUpdateVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
