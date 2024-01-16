namespace Shopwise.Entities
{
    public class OfferSection
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
    }
}
