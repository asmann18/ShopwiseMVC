namespace Shopwise.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ProductTag>? ProduductTags { get; set; }
}
