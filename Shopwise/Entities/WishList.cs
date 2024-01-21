namespace Shopwise.Entities
{
    public class WishList
    {
        public int Id { get; set; }
        public CustomUser CustomUser { get; set; }
        public string CustomUserId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
