using System.Net;
using System.Text;
using System.Text.Json;
using EShop.Api.IntegrationTests.Infrastructure;
using EShop.Catalog.Domain.Products.Entities;

namespace EShop.Api.IntegrationTests.Modules;

public class CatalogIntegrationTests : IntegrationTestBase
{
    public CatalogIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateProduct_WithValidData_ShouldReturn201()
    {
        // Arrange
        var productData = new
        {
            Name = "Integration Test Product",
            Description = "A product created during integration testing",
            ImageFile = "test-product.jpg",
            Price = 199.99m,
            Category = new[] { "Electronics", "Testing" }
        };

        var json = JsonSerializer.Serialize(productData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PostAsync("/products", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        result.GetProperty("id").GetString().ShouldNotBeNull();
        result.GetProperty("name").GetString().ShouldBe(productData.Name);
        result.GetProperty("price").GetDecimal().ShouldBe(productData.Price);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnProductsList()
    {
        // Arrange - Create a test product first
        await CreateTestProduct("Get Products Test Product", 99.99m);

        // Act
        var response = await HttpClient.GetAsync("/products");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        result.ValueKind.ShouldBe(JsonValueKind.Array);
        result.GetArrayLength().ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetProductById_WithExistingProduct_ShouldReturnProduct()
    {
        // Arrange
        var createdProduct = await CreateTestProduct("GetById Test Product", 149.99m);
        var productId = createdProduct.GetProperty("id").GetString();

        // Act
        var response = await HttpClient.GetAsync($"/products/{productId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        result.GetProperty("id").GetString().ShouldBe(productId);
        result.GetProperty("name").GetString().ShouldBe("GetById Test Product");
        result.GetProperty("price").GetDecimal().ShouldBe(149.99m);
    }

    [Fact]
    public async Task GetProductById_WithNonExistentProduct_ShouldReturn404()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/products/{nonExistentId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProduct_WithValidData_ShouldReturn200()
    {
        // Arrange
        var createdProduct = await CreateTestProduct("Update Test Product", 199.99m);
        var productId = createdProduct.GetProperty("id").GetString();

        var updateData = new
        {
            Name = "Updated Product Name",
            Description = "Updated description",
            ImageFile = "updated-image.jpg",
            Price = 299.99m,
            Category = new[] { "Updated", "Categories" }
        };

        var json = JsonSerializer.Serialize(updateData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PutAsync($"/products/{productId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        // Verify the update
        var getResponse = await HttpClient.GetAsync($"/products/{productId}");
        var responseContent = await getResponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        result.GetProperty("name").GetString().ShouldBe(updateData.Name);
        result.GetProperty("price").GetDecimal().ShouldBe(updateData.Price);
        result.GetProperty("description").GetString().ShouldBe(updateData.Description);
    }

    [Fact]
    public async Task DeleteProduct_WithExistingProduct_ShouldReturn204()
    {
        // Arrange
        var createdProduct = await CreateTestProduct("Delete Test Product", 99.99m);
        var productId = createdProduct.GetProperty("id").GetString();

        // Act
        var response = await HttpClient.DeleteAsync($"/products/{productId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        // Verify deletion
        var getResponse = await HttpClient.GetAsync($"/products/{productId}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProductsByCategory_WithValidCategory_ShouldReturnFilteredProducts()
    {
        // Arrange
        await CreateTestProduct("Electronics Product", 299.99m, new[] { "Electronics", "Technology" });
        await CreateTestProduct("Books Product", 19.99m, new[] { "Books", "Education" });

        // Act
        var response = await HttpClient.GetAsync("/products/category/Electronics");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        result.ValueKind.ShouldBe(JsonValueKind.Array);
        
        foreach (var product in result.EnumerateArray())
        {
            var categories = product.GetProperty("category").EnumerateArray()
                .Select(c => c.GetString()).ToArray();
            categories.ShouldContain("Electronics");
        }
    }

    [Fact]
    public async Task CreateProduct_WithInvalidData_ShouldReturn400()
    {
        // Arrange
        var invalidProductData = new
        {
            Name = "", // Invalid empty name
            Description = "Test description",
            ImageFile = "test.jpg",
            Price = -10.00m, // Invalid negative price
            Category = new[] { "Test" }
        };

        var json = JsonSerializer.Serialize(invalidProductData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PostAsync("/products", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateProduct_WithNonExistentProduct_ShouldReturn404()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateData = new
        {
            Name = "Updated Product",
            Description = "Updated description",
            ImageFile = "updated.jpg",
            Price = 199.99m,
            Category = new[] { "Test" }
        };

        var json = JsonSerializer.Serialize(updateData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PutAsync($"/products/{nonExistentId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<JsonElement> CreateTestProduct(string name, decimal price, string[]? categories = null)
    {
        var productData = new
        {
            Name = name,
            Description = $"Test description for {name}",
            ImageFile = "test-image.jpg",
            Price = price,
            Category = categories ?? new[] { "Test", "Integration" }
        };

        var json = JsonSerializer.Serialize(productData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await HttpClient.PostAsync("/products", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<JsonElement>(responseContent);
    }
}