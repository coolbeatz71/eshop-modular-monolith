using System.Net;
using System.Text;
using System.Text.Json;
using EShop.Api.IntegrationTests.Builders;
using EShop.Api.IntegrationTests.Infrastructure;
using EShop.Basket.Domain.Basket.Dtos;
using EShop.Catalog.Domain.Products.Entities;
using EShop.Catalog.DataSource;

namespace EShop.Api.IntegrationTests.Modules;

public class BasketIntegrationTests : IntegrationTestBase
{
    public BasketIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await SeedTestData();
    }

    private async Task SeedTestData()
    {
        // Add some products for testing basket operations
        using var catalogContext = GetService<CatalogDbContext>();
        
        var products = new[]
        {
            ProductTestDataBuilder.New()
                .WithName("Test Product 1")
                .WithPrice(99.99m)
                .WithElectronicsCategory()
                .Build(),
            ProductTestDataBuilder.New()
                .WithName("Test Product 2")
                .WithPrice(149.99m)
                .WithElectronicsCategory()
                .Build()
        };

        catalogContext.Products.AddRange(products);
        await catalogContext.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateBasket_WithValidData_ShouldReturn200()
    {
        // Arrange
        var catalogContext = GetService<CatalogDbContext>();
        var product = await catalogContext.Products.FirstAsync();

        var basketDto = new ShoppingCartDto
        {
            UserName = "testuser@example.com",
            Items = new List<ShoppingCartItemDto>
            {
                new()
                {
                    ProductId = product.Id,
                    Quantity = 2,
                    Color = "Red"
                }
            }
        };

        var json = JsonSerializer.Serialize(basketDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PostAsync("/basket", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.ShouldNotBeNull();
        
        var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
        result.GetProperty("id").GetString().ShouldNotBeNull();
    }

    [Fact]
    public async Task GetBasket_WithExistingBasket_ShouldReturnBasket()
    {
        // Arrange
        var catalogContext = GetService<CatalogDbContext>();
        var product = await catalogContext.Products.FirstAsync();
        var userName = "getbaskettest@example.com";

        // First create a basket
        var basketDto = new ShoppingCartDto
        {
            UserName = userName,
            Items = new List<ShoppingCartItemDto>
            {
                new()
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    Color = "Blue"
                }
            }
        };

        var json = JsonSerializer.Serialize(basketDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await HttpClient.PostAsync("/basket", content);

        // Act
        var response = await HttpClient.GetAsync($"/basket/{userName}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        result.GetProperty("userName").GetString().ShouldBe(userName);
        result.GetProperty("items").GetArrayLength().ShouldBe(1);
    }

    [Fact]
    public async Task GetBasket_WithNonExistentBasket_ShouldReturn404()
    {
        // Arrange
        var nonExistentUserName = "nonexistent@example.com";

        // Act
        var response = await HttpClient.GetAsync($"/basket/{nonExistentUserName}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteBasket_WithExistingBasket_ShouldReturn200()
    {
        // Arrange
        var catalogContext = GetService<CatalogDbContext>();
        var product = await catalogContext.Products.FirstAsync();
        var userName = "deletetest@example.com";

        // First create a basket
        var basketDto = new ShoppingCartDto
        {
            UserName = userName,
            Items = new List<ShoppingCartItemDto>
            {
                new()
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    Color = "Green"
                }
            }
        };

        var json = JsonSerializer.Serialize(basketDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await HttpClient.PostAsync("/basket", content);

        // Act
        var response = await HttpClient.DeleteAsync($"/basket/{userName}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        // Verify basket is deleted
        var getResponse = await HttpClient.GetAsync($"/basket/{userName}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddItemToBasket_WithValidData_ShouldReturn200()
    {
        // Arrange
        var catalogContext = GetService<CatalogDbContext>();
        var products = await catalogContext.Products.Take(2).ToListAsync();
        var userName = "additemtest@example.com";

        // First create a basket with one item
        var basketDto = new ShoppingCartDto
        {
            UserName = userName,
            Items = new List<ShoppingCartItemDto>
            {
                new()
                {
                    ProductId = products[0].Id,
                    Quantity = 1,
                    Color = "Red"
                }
            }
        };

        var json = JsonSerializer.Serialize(basketDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await HttpClient.PostAsync("/basket", content);

        // Now add another item
        var addItemDto = new ShoppingCartItemDto
        {
            ProductId = products[1].Id,
            Quantity = 2,
            Color = "Blue"
        };

        var addItemJson = JsonSerializer.Serialize(addItemDto);
        var addItemContent = new StringContent(addItemJson, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PostAsync($"/basket/{userName}/items", addItemContent);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        // Verify basket now has two items
        var getResponse = await HttpClient.GetAsync($"/basket/{userName}");
        var responseContent = await getResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        result.GetProperty("items").GetArrayLength().ShouldBe(2);
    }

    [Fact]
    public async Task CreateBasket_WithNonExistentProduct_ShouldReturn404()
    {
        // Arrange
        var basketDto = new ShoppingCartDto
        {
            UserName = "invalidproducttest@example.com",
            Items = new List<ShoppingCartItemDto>
            {
                new()
                {
                    ProductId = Guid.NewGuid(), // Non-existent product
                    Quantity = 1,
                    Color = "Red"
                }
            }
        };

        var json = JsonSerializer.Serialize(basketDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PostAsync("/basket", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateBasket_WithInvalidData_ShouldReturn400()
    {
        // Arrange
        var basketDto = new ShoppingCartDto
        {
            UserName = "", // Invalid empty username
            Items = new List<ShoppingCartItemDto>()
        };

        var json = JsonSerializer.Serialize(basketDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PostAsync("/basket", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}