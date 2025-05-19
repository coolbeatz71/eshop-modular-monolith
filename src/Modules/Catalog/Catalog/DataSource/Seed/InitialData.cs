using Bogus;
using EShop.Catalog.Products.Models;

namespace EShop.Catalog.DataSource.Seed
{
    public static class InitialData
    {
        private static readonly Faker Faker = new Faker();

        public static IEnumerable<Product> Products =>
            Enumerable.Range(1, 5).Select(_ =>
                Product.Create(
                    Guid.NewGuid(),
                    Faker.Commerce.ProductName(),
                    Faker.Commerce.ProductDescription(),
                    Faker.Image.PicsumUrl(),
                    decimal.Parse(Faker.Commerce.Price()),
                    Faker.Commerce.Categories(1).ToList()
                )
            );
    }
}