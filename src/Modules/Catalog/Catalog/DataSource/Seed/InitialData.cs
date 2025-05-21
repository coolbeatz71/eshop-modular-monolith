using Bogus;
using EShop.Catalog.Products.Entities;

namespace EShop.Catalog.DataSource.Seed;

/// <summary>
/// Provides initial seed data for the catalog.
/// </summary>
public static class InitialData
{
    /// <summary>
    /// The <see cref="Faker"/> instance used to generate fake product data.
    /// </summary>
    private static readonly Faker Faker = new Faker();

    /// <summary>
    /// Gets a collection of randomly generated <see cref="ProductEntity"/> instances for seeding.
    /// </summary>
    /// <remarks>
    /// Generates 5 sample products with random names, descriptions, images, prices, and categories using Bogus.
    /// </remarks>
    public static IEnumerable<ProductEntity> Products =>
        Enumerable.Range(1, 5).Select(_ =>
            ProductEntity.Create(
                Guid.NewGuid(),
                Faker.Commerce.ProductName(),
                Faker.Commerce.ProductDescription(),
                Faker.Image.PicsumUrl(),
                decimal.Parse(Faker.Commerce.Price()),
                Faker.Commerce.Categories(1).ToList()
            )
        );
}
