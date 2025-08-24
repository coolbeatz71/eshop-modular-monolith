using Bogus;
using EShop.Catalog.Domain.Products.Entities;

namespace EShop.Catalog.UnitTests.Builders;

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

    public ProductTestDataBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProductTestDataBuilder WithImageFile(string imageFile)
    {
        _imageFile = imageFile;
        return this;
    }

    public ProductTestDataBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductTestDataBuilder WithCategory(params string[] categories)
    {
        _category = categories.ToList();
        return this;
    }

    public ProductTestDataBuilder WithCategories(List<string> categories)
    {
        _category = categories;
        return this;
    }

    public ProductTestDataBuilder WithRandomData()
    {
        _id = Faker.Random.Guid();
        _name = Faker.Commerce.ProductName();
        _description = Faker.Lorem.Sentence(10);
        _imageFile = Faker.Image.PicsumUrl(300, 300);
        _price = Faker.Random.Decimal(10, 5000);
        _category = Faker.Make(Faker.Random.Int(1, 4), () => Faker.Commerce.Categories(1)[0]).Distinct().ToList();
        return this;
    }

    public ProductTestDataBuilder WithElectronicsCategory()
    {
        _category = ["Electronics", "Technology"];
        return this;
    }

    public ProductTestDataBuilder WithClothingCategory()
    {
        _category = ["Fashion", "Clothing"];
        return this;
    }

    public ProductTestDataBuilder WithBooksCategory()
    {
        _category = ["Books", "Education"];
        return this;
    }

    public ProductTestDataBuilder WithRandomCategories(int count = 2)
    {
        _category = Faker.Make(count, () => Faker.Commerce.Categories(1)[0]).Distinct().ToList();
        return this;
    }

    public ProductEntity Build()
    {
        EnsureDefaultValues();

        return ProductEntity.Create(
            _id,
            _name,
            _description,
            _imageFile,
            _price,
            _category
        );
    }

    private void EnsureDefaultValues()
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
    }

    public static List<ProductEntity> CreateMany(int count)
    {
        var products = new List<ProductEntity>();
        for (int i = 0; i < count; i++)
        {
            products.Add(New().WithRandomData().Build());
        }
        return products;
    }

    public static ProductEntity CreateElectronicsProduct()
    {
        return New()
            .WithRandomData()
            .WithElectronicsCategory()
            .Build();
    }

    public static ProductEntity CreateClothingProduct()
    {
        return New()
            .WithRandomData()
            .WithClothingCategory()
            .Build();
    }

    public static ProductEntity CreateBookProduct()
    {
        return New()
            .WithRandomData()
            .WithBooksCategory()
            .Build();
    }
}