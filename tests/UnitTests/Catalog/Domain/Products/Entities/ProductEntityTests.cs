using EShop.Catalog.Domain.Products.Entities;
using EShop.Catalog.Domain.Products.Events;
using EShop.Catalog.UnitTests.Builders;

namespace EShop.Catalog.UnitTests.Domain.Products.Entities;

public class ProductEntityTests : UnitTestBase
{
    [Fact]
    public void Create_WithValidParameters_ShouldCreateProduct()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Product";
        var description = "Test Description";
        var imageFile = "test-image.jpg";
        var price = 99.99m;
        var categories = new List<string> { "Electronics", "Computers" };

        // Act
        var result = ProductEntity.Create(id, name, description, imageFile, price, categories);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
        result.Name.ShouldBe(name);
        result.Description.ShouldBe(description);
        result.ImageFile.ShouldBe(imageFile);
        result.Price.ShouldBe(price);
        result.Category.ShouldBe(categories);
    }

    [Fact]
    public void Create_ShouldRaiseProductCreatedEvent()
    {
        // Arrange
        var productData = ProductTestDataBuilder.New().WithRandomData();

        // Act
        var product = productData.Build();

        // Assert
        var domainEvents = product.DomainEvents;
        domainEvents.ShouldHaveCount(1);
        var createdEvent = domainEvents.First().Should().BeOfType<ProductCreatedEvent>().Subject;
        createdEvent.ProductEntity.ShouldBe(product);
    }

    [Fact]
    public void Create_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categories = new List<string> { "Test" };

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ProductEntity.Create(id, null!, "desc", "image.jpg", 99.99m, categories));
        
        exception.ParamName.ShouldBe("name");
    }
    
    [Fact]
    public void Create_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categories = new List<string> { "Test" };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ProductEntity.Create(id, "", "desc", "image.jpg", 99.99m, categories));
        
        exception.ParamName.ShouldBe("name");
    }
    
    [Fact]
    public void Create_WithWhitespaceName_ShouldCreateValidProduct()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "   ";
        var categories = new List<string> { "Test" };

        // Act
        var product = ProductEntity.Create(id, name, "desc", "image.jpg", 99.99m, categories);

        // Assert
        product.ShouldNotBeNull();
        product.Name.ShouldBe(name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-99.99)]
    public void Create_WithNegativePrice_ShouldThrowArgumentOutOfRangeException(decimal negativePrice)
    {
        // Arrange
        var id = Guid.NewGuid();
        var categories = new List<string> { "Test" };

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ProductEntity.Create(id, "Test Product", "desc", "image.jpg", negativePrice, categories));
        
        exception.ParamName.ShouldBe("price");
    }

    [Fact]
    public void Create_WithZeroPrice_ShouldSucceed()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categories = new List<string> { "Test" };

        // Act
        var result = ProductEntity.Create(id, "Free Product", "desc", "image.jpg", 0m, categories);

        // Assert
        result.ShouldNotBeNull();
        result.Price.ShouldBe(0m);
    }

    [Fact]
    public void Update_WithSamePrice_ShouldNotRaisePriceChangedEvent()
    {
        // Arrange
        var product = ProductTestDataBuilder.New().WithPrice(100m).Build();
        product.ClearDomainEvents(); // Clear initial events

        // Act
        product.Update("Updated Name", "Updated Description", "updated-image.jpg", 100m, ["Updated Category"]);

        // Assert
        var domainEvents = product.DomainEvents;
        domainEvents.ShouldBeEmpty();
        
        product.Name.ShouldBe("Updated Name");
        product.Description.ShouldBe("Updated Description");
        product.ImageFile.ShouldBe("updated-image.jpg");
        product.Category.ShouldContain("Updated Category");
    }

    [Fact]
    public void Update_WithDifferentPrice_ShouldRaisePriceChangedEvent()
    {
        // Arrange
        var product = ProductTestDataBuilder.New().WithPrice(100m).Build();
        product.ClearDomainEvents(); // Clear initial events
        var newPrice = 150m;

        // Act
        product.Update("Updated Name", "Updated Description", "updated-image.jpg", newPrice, ["Updated Category"]);

        // Assert
        var domainEvents = product.DomainEvents;
        domainEvents.ShouldHaveCount(1);
        
        var priceChangedEvent = domainEvents.First().Should().BeOfType<ProductPriceChangedEvent>().Subject;
        priceChangedEvent.ProductEntity.ShouldBe(product);
        
        product.Price.ShouldBe(newPrice);
        product.Name.ShouldBe("Updated Name");
    }

    [Fact]
    public void Update_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var product = ProductTestDataBuilder.New().WithRandomData().Build();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            product.Update(null!, "desc", "image.jpg", 99.99m, ["Test"]));
        
        exception.ParamName.ShouldBe("name");
    }
    
    [Fact]
    public void Update_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var product = ProductTestDataBuilder.New().WithRandomData().Build();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            product.Update("", "desc", "image.jpg", 99.99m, ["Test"]));
        
        exception.ParamName.ShouldBe("name");
    }
    
    [Fact]
    public void Update_WithWhitespaceName_ShouldUpdateProduct()
    {
        // Arrange
        var product = ProductTestDataBuilder.New().WithRandomData().Build();
        var name = "   ";

        // Act
        product.Update(name, "desc", "image.jpg", 99.99m, ["Test"]);

        // Assert
        product.Name.ShouldBe(name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-99.99)]
    public void Update_WithNegativePrice_ShouldThrowArgumentOutOfRangeException(decimal negativePrice)
    {
        // Arrange
        var product = ProductTestDataBuilder.New().WithRandomData().Build();

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            product.Update("Test Product", "desc", "image.jpg", negativePrice, ["Test"]));
        
        exception.ParamName.ShouldBe("price");
    }

    [Fact]
    public void Update_AllProperties_ShouldUpdateAllFields()
    {
        // Arrange
        var product = ProductTestDataBuilder.New().WithRandomData().Build();
        var newName = "Updated Product";
        var newDescription = "Updated Description";
        var newImageFile = "updated-image.jpg";
        var newPrice = 199.99m;
        var newCategories = new List<string> { "Updated", "Categories" };

        // Act
        product.Update(newName, newDescription, newImageFile, newPrice, newCategories);

        // Assert
        product.Name.ShouldBe(newName);
        product.Description.ShouldBe(newDescription);
        product.ImageFile.ShouldBe(newImageFile);
        product.Price.ShouldBe(newPrice);
        product.Category.ShouldBe(newCategories);
    }

    [Fact]
    public void Update_WithEmptyCategories_ShouldAllowEmptyList()
    {
        // Arrange
        var product = ProductTestDataBuilder.New().WithRandomData().Build();

        // Act
        product.Update("Test Product", "desc", "image.jpg", 99.99m, []);

        // Assert
        product.Category.ShouldBeEmpty();
    }
}