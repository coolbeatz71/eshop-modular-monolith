using EShop.Catalog.Products.Events;
using EShop.Shared.Domain;

namespace EShop.Catalog.Products.Entities;

/// <summary>
/// Represents a product in the catalog with support for domain events and state management.
/// </summary>
/// <remarks>
/// This entity is an aggregate root with a unique identifier and raises domain events on creation and price change.
/// </remarks>
public class ProductEntity : Aggregate<Guid>
{
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets the description of the product.
    /// </summary>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// Gets the image file URL or path associated with the product.
    /// </summary>
    public string ImageFile { get; private set; } = null!;

    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets the list of categories the product belongs to.
    /// </summary>
    public List<string> Category { get; private set; } = [];

    /// <summary>
    /// Creates a new <see cref="ProductEntity"/> with the specified properties and raises a <see cref="ProductCreatedEvent"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="name">The name of the product. Cannot be null or empty.</param>
    /// <param name="description">The product description.</param>
    /// <param name="imageFile">The image file URL or path.</param>
    /// <param name="price">The price of the product. Must be non-negative.</param>
    /// <param name="category">The list of categories the product belongs to.</param>
    /// <returns>A new instance of <see cref="ProductEntity"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="price"/> is negative.</exception>
    /// <example>
    /// <code>
    /// var product = ProductEntity.Create(
    ///     Guid.NewGuid(),
    ///     "Laptop",
    ///     "High-end gaming laptop",
    ///     "images/laptop.jpg",
    ///     1599.99m,
    ///     new List&lt;string&gt; { "Electronics", "Computers" }
    /// );
    /// </code>
    /// </example>
    public static ProductEntity Create(
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

        var product = new ProductEntity
        {
            Id = id,
            Name = name,
            Description = description,
            ImageFile = imageFile,
            Price = price,
            Category = category
        };

        product.AddDomainEvent(new ProductCreatedEvent(product));
        return product;
    }

    /// <summary>
    /// Updates the product properties and raises a <see cref="ProductPriceChangedEvent"/> if the price has changed.
    /// </summary>
    /// <param name="name">The updated product name. Cannot be null or empty.</param>
    /// <param name="description">The updated product description.</param>
    /// <param name="imageFile">The updated image file URL or path.</param>
    /// <param name="price">The updated price. Must be non-negative.</param>
    /// <param name="category">The updated list of categories.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="price"/> is negative.</exception>
    /// <example>
    /// <code>
    /// product.Update(
    ///     "Updated Laptop",
    ///     "Improved specs and performance",
    ///     "images/laptop-v2.jpg",
    ///     1699.99m,
    ///     new List&lt;string&gt; { "Electronics", "Laptops" }
    /// );
    /// </code>
    /// </example>
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

        if (Price == price) return;

        Price = price;
        AddDomainEvent(new ProductPriceChangedEvent(this));
    }
}
