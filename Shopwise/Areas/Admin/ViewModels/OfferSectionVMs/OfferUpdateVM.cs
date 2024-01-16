namespace Shopwise.Areas.Admin.ViewModels.OfferSectionVMs
{
    public class OfferUpdateVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }

    }
}