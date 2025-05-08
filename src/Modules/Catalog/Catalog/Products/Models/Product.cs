using EShop.Catalog.Products.Events;
using EShop.Shared.Domain;

namespace EShop.Catalog.Products.Models;

public class Product: Aggregate<Guid>
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string ImageFile { get; private set; } = null!;
    public decimal Price { get; private set; }
    public List<string> Category { get; private set; } = [];

    public static Product Create(
        Guid id, 
        string name, 
        string description, 
        string imageFile, 
        decimal price, 
        List<string> category
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        
        var product = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            ImageFile = imageFile,
            Price = price,
            Category = category
        };
        
        // raise the product created domain event
        product.AddDomainEvent(new ProductCreatedEvent(product));
        
        return product;
    }

    public void Update(
        string name, 
        string description, 
        string imageFile, 
        decimal price, 
        List<string> category
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegative(price);

        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;
    
        // raise product price changed domain event when the price changes
        if (Price == price) return;
        Price = price;
        AddDomainEvent(new ProductPriceChangedEvent(this));
    }
}