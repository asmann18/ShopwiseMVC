using Microsoft.CodeAnalysis.Options;

namespace Shopwise.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public string Path { get; set; }
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public bool? IsPrimary { get; set; } = null;
}
