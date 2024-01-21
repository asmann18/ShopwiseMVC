using System.ComponentModel.DataAnnotations;

namespace Shopwise.Areas.Admin.ViewModels;

public class ProductCreateVM
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public decimal Price { get; set; }
    [Range(0, 100)]
    public byte Offer { get; set; }
    public string Warranty { get; set; }
    public string ReturnPolicy { get; set; }
    public string CashOnDelivery { get; set; }
    public string SKU { get; set; }
    public string FacebookLink { get; set; }
    public string TwitterLink { get; set; }
    public int CategoryId { get; set; }
    public List<int> TagIds { get; set; }
    public ICollection<IFormFile> ProductImages { get; set; }
}
