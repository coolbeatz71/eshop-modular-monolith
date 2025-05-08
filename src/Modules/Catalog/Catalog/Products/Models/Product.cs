using EShop.Shared.Domain;

namespace EShop.Catalog.Products.Models;

public class Product: Entity<Guid>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageFile { get; set; } = null!;
    public decimal Price { get; set; }
    public List<string> Category { get; set; } = [];
}