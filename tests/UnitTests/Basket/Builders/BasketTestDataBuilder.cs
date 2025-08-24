using Bogus;
using EShop.Basket.Domain.Basket.Entities;

namespace EShop.Basket.UnitTests.Builders;

public class BasketTestDataBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _userName = string.Empty;

    private static readonly Faker Faker = new();

    public static BasketTestDataBuilder New() => new();

    public BasketTestDataBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public BasketTestDataBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public BasketTestDataBuilder WithRandomUserName()
    {
        _userName = Faker.Internet.UserName();
        return this;
    }

    public BasketTestDataBuilder WithRandomData()
    {
        _id = Faker.Random.Guid();
        _userName = Faker.Internet.UserName();
        return this;
    }

    public ShoppingCartEntity Build()
    {
        if (string.IsNullOrEmpty(_userName))
        {
            _userName = Faker.Internet.UserName();
        }

        return ShoppingCartEntity.Create(_id, _userName);
    }

    public static List<ShoppingCartEntity> CreateMany(int count)
    {
        var baskets = new List<ShoppingCartEntity>();
        for (int i = 0; i < count; i++)
        {
            baskets.Add(New().WithRandomData().Build());
        }
        return baskets;
    }
}