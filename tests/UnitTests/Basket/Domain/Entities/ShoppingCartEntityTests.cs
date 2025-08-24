using EShop.Basket.Domain.Basket.Entities;
using EShop.Basket.UnitTests.Builders;

namespace EShop.Basket.UnitTests.Domain.Entities;

public class ShoppingCartEntityTests : UnitTestBase
{
    [Fact]
    public void Create_WithValidParameters_ShouldCreateShoppingCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var userName = "testuser";

        // Act
        var result = ShoppingCartEntity.Create(cartId, userName);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(cartId);
        result.UserName.ShouldBe(userName);
        result.Items.ShouldBeEmpty();
        result.TotalPrice.ShouldBe(0);
    }

    [Fact]
    public void Create_WithNullUserName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var cartId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            ShoppingCartEntity.Create(cartId, null!));
        
        exception.ParamName.ShouldBe("userName");
    }
    
    [Fact]
    public void Create_WithEmptyUserName_ShouldThrowArgumentException()
    {
        // Arrange
        var cartId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            ShoppingCartEntity.Create(cartId, ""));
        
        exception.ParamName.ShouldBe("userName");
    }
    
    [Fact]
    public void Create_WithWhitespaceUserName_ShouldCreateValidCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var userName = "   ";

        // Act
        var result = ShoppingCartEntity.Create(cartId, userName);

        // Assert
        result.ShouldNotBeNull();
        result.UserName.ShouldBe(userName);
    }

    [Fact]
    public void AddItem_WithValidParameters_ShouldAddItemToCart()
    {
        // Arrange
        var cart = BasketTestDataBuilder.New().WithRandomData().Build();
        var productId = Guid.NewGuid();
        var quantity = 2;
        var color = "Red";
        var price = 99.99m;
        var productName = "Test Product";

        // Act
        cart.AddItem(productId, quantity, color, price, productName);

        // Assert
        cart.Items.ShouldHaveCount(1);
        var item = cart.Items.First();
        item.ProductId.ShouldBe(productId);
        item.Quantity.ShouldBe(quantity);
        item.Color.ShouldBe(color);
        item.Price.ShouldBe(price);
        item.ProductName.ShouldBe(productName);
        cart.TotalPrice.ShouldBe(price * quantity);
    }

    [Fact]
    public void AddItem_WithExistingProductId_ShouldUpdateQuantity()
    {
        // Arrange
        var cart = BasketTestDataBuilder.New().WithRandomData().Build();
        var productId = Guid.NewGuid();
        var initialQuantity = 1;
        var additionalQuantity = 3;
        var price = 50.00m;

        cart.AddItem(productId, initialQuantity, "Blue", price, "Test Product");

        // Act
        cart.AddItem(productId, additionalQuantity, "Blue", price, "Test Product");

        // Assert
        cart.Items.ShouldHaveCount(1);
        cart.Items.First().Quantity.ShouldBe(initialQuantity + additionalQuantity);
        cart.TotalPrice.ShouldBe(price * (initialQuantity + additionalQuantity));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void AddItem_WithInvalidQuantity_ShouldThrowArgumentOutOfRangeException(int invalidQuantity)
    {
        // Arrange
        var cart = BasketTestDataBuilder.New().WithRandomData().Build();
        var productId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            cart.AddItem(productId, invalidQuantity, "Red", 99.99m, "Test Product"));
        
        exception.ParamName.ShouldBe("quantity");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99.99)]
    public void AddItem_WithInvalidPrice_ShouldThrowArgumentOutOfRangeException(decimal invalidPrice)
    {
        // Arrange
        var cart = BasketTestDataBuilder.New().WithRandomData().Build();
        var productId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            cart.AddItem(productId, 1, "Red", invalidPrice, "Test Product"));
        
        exception.ParamName.ShouldBe("price");
    }

    [Fact]
    public void RemoveItem_WithExistingProductId_ShouldRemoveItem()
    {
        // Arrange
        var cart = BasketTestDataBuilder.New().WithRandomData().Build();
        var productId = Guid.NewGuid();
        cart.AddItem(productId, 1, "Red", 99.99m, "Test Product");

        // Act
        cart.RemoveItem(productId);

        // Assert
        cart.Items.ShouldBeEmpty();
        cart.TotalPrice.ShouldBe(0);
    }

    [Fact]
    public void RemoveItem_WithNonExistingProductId_ShouldNotThrow()
    {
        // Arrange
        var cart = BasketTestDataBuilder.New().WithRandomData().Build();
        var nonExistingProductId = Guid.NewGuid();

        // Act & Assert
        Action act = () => cart.RemoveItem(nonExistingProductId);
        act.ShouldNotThrow();
        cart.Items.ShouldBeEmpty();
    }

    [Fact]
    public void TotalPrice_WithMultipleItems_ShouldCalculateCorrectTotal()
    {
        // Arrange
        var cart = BasketTestDataBuilder.New().WithRandomData().Build();
        
        cart.AddItem(Guid.NewGuid(), 2, "Red", 50.00m, "Product 1");
        cart.AddItem(Guid.NewGuid(), 1, "Blue", 75.00m, "Product 2");
        cart.AddItem(Guid.NewGuid(), 3, "Green", 25.00m, "Product 3");

        var expectedTotal = (2 * 50.00m) + (1 * 75.00m) + (3 * 25.00m);

        // Act & Assert
        cart.TotalPrice.ShouldBe(expectedTotal);
    }
}