using Shopwise.Entities;

namespace Shopwise.ViewModels
{
    public class HomeVM
    {
        public List<Slider>  Sliders{ get; set; }
        public List<OfferSection>  OfferSections{ get; set; }
        public OfferSection  SpecialOffer{ get; set; }
        public List<Category>  TopCategories{ get; set; }
        public List<Product>  Products{ get; set; }
        public List<Product>  TrendingProducts{ get; set; }

    }
}
