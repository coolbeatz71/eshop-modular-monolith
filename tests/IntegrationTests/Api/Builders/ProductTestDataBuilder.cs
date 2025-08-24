using Bogus;
using EShop.Catalog.Domain.Products.Entities;

namespace EShop.Api.IntegrationTests.Builders;

public class ProductTestDataBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _imageFile = string.Empty;
    private decimal _price = 0;
    private List<string> _category = [];

    private static readonly Faker Faker = new();

    public static ProductTestDataBuilder New() => new();

    public ProductTestDataBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ProductTestDataBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductTestDataBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductTestDataBuilder WithElectronicsCategory()
    {
        _category = ["Electronics", "Technology"];
        return this;
    }

    public ProductEntity Build()
    {
        if (string.IsNullOrEmpty(_name))
            _name = Faker.Commerce.ProductName();
        
        if (string.IsNullOrEmpty(_description))
            _description = Faker.Lorem.Sentence(10);
        
        if (string.IsNullOrEmpty(_imageFile))
            _imageFile = Faker.Image.PicsumUrl(300, 300);
        
        if (_price == 0)
            _price = Faker.Random.Decimal(10, 5000);
        
        if (_category.Count == 0)
            _category = [Faker.Commerce.Categories(1)[0]];

        return ProductEntity.Create(
            _id,
            _name,
            _description,
            _imageFile,
            _price,
            _category
        );
    }
}