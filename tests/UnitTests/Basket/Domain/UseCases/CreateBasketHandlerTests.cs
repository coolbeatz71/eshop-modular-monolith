using EShop.Basket.Domain.Basket.Dtos;
using EShop.Basket.Domain.Basket.Entities;
using EShop.Basket.Domain.Basket.Repositories;
using EShop.Basket.Domain.Basket.UseCases.CreateBasket;
using EShop.Basket.UnitTests.Builders;
using EShop.Basket.UnitTests.Mocks;
using EShop.Catalog.Contracts.Domain.Products.Dtos;
using EShop.Catalog.Contracts.Domain.Products.UseCases.GetProductById;
using EShop.Shared.Exceptions;
using MediatR;

namespace EShop.Basket.UnitTests.Domain.UseCases;

public class CreateBasketHandlerTests : UnitTestBase
{
    private readonly Mock<IBasketRepository> _mockRepository;
    private readonly Mock<ISender> _mockSender;
    private readonly CreateBasketHandler _handler;

    public CreateBasketHandlerTests()
    {
        _mockRepository = MockBasketRepository.Create();
        _mockSender = new Mock<ISender>();
        _handler = new CreateBasketHandler(_mockRepository.Object, _mockSender.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateBasketSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const string userName = "testuser";
        const int quantity = 2;
        const string color = "Red";
        
        var productDto = ProductTestDataBuilder.New()
            .WithId(productId)
            .WithName("Test Product")
            .WithPrice(99.99m)
            .Build();

        var shoppingCartDto = new ShoppingCartDto
        {
            UserName = userName,
            Items = new List<ShoppingCartItemDto>
            {
                new()
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Color = color
                }
            }
        };

        var command = new CreateBasketCommand(shoppingCartDto);

        _mockSender.Setup(s => s.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new GetProductByIdResult(new ProductDto
                  {
                      Id = productDto.Id,
                      Name = productDto.Name,
                      Description = productDto.Description,
                      ImageFile = productDto.ImageFile,
                      Price = productDto.Price,
                      Category = productDto.Category
                  }));

        _mockRepository.Setup(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync((ShoppingCartEntity basket, CancellationToken _) => basket);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);

        _mockSender.Verify(s => s.Send(
            It.Is<GetProductByIdQuery>(q => q.ProductId == productId.ToString()),
            It.IsAny<CancellationToken>()), Times.Once);

        _mockRepository.Verify(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentProduct_ShouldThrowNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const string userName = "testuser";
        
        var shoppingCartDto = new ShoppingCartDto
        {
            UserName = userName,
            Items = new List<ShoppingCartItemDto>
            {
                new()
                {
                    ProductId = productId,
                    Quantity = 1,
                    Color = "Red"
                }
            }
        };

        var command = new CreateBasketCommand(shoppingCartDto);

        _mockSender.Setup(s => s.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))!
                  .ReturnsAsync(null as GetProductByIdResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        exception.Message.ShouldContain("ShoppingCart");
        exception.Message.ShouldContain(productId.ToString());

        _mockRepository.Verify(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithMultipleItems_ShouldCreateBasketWithAllItems()
    {
        // Arrange
        var product1Id = Guid.NewGuid();
        var product2Id = Guid.NewGuid();
        const string userName = "testuser";
        
        var product1 = ProductTestDataBuilder.New().WithId(product1Id).WithName("Product 1").WithPrice(50.00m).Build();
        var product2 = ProductTestDataBuilder.New().WithId(product2Id).WithName("Product 2").WithPrice(75.00m).Build();

        var shoppingCartDto = new ShoppingCartDto
        {
            UserName = userName,
            Items = new List<ShoppingCartItemDto>
            {
                new() { ProductId = product1Id, Quantity = 2, Color = "Red" },
                new() { ProductId = product2Id, Quantity = 1, Color = "Blue" }
            }
        };

        var command = new CreateBasketCommand(shoppingCartDto);

        _mockSender.Setup(s => s.Send(It.Is<GetProductByIdQuery>(q => q.ProductId == product1Id.ToString()), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new GetProductByIdResult(new ProductDto
                  {
                      Id = product1.Id,
                      Name = product1.Name,
                      Description = product1.Description,
                      ImageFile = product1.ImageFile,
                      Price = product1.Price,
                      Category = product1.Category
                  }));

        _mockSender.Setup(s => s.Send(It.Is<GetProductByIdQuery>(q => q.ProductId == product2Id.ToString()), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new GetProductByIdResult(new ProductDto
                  {
                      Id = product2.Id,
                      Name = product2.Name,
                      Description = product2.Description,
                      ImageFile = product2.ImageFile,
                      Price = product2.Price,
                      Category = product2.Category
                  }));

        ShoppingCartEntity? capturedBasket = null;
        _mockRepository.Setup(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()))
                      .Callback<ShoppingCartEntity, CancellationToken>((basket, _) => capturedBasket = basket)
                      .ReturnsAsync((ShoppingCartEntity basket, CancellationToken _) => basket);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        capturedBasket.ShouldNotBeNull();
        capturedBasket!.Items.ShouldHaveCount(2);
        capturedBasket!.UserName.ShouldBe(userName);
        
        const decimal expectedTotal = (2 * 50.00m) + (1 * 75.00m);
        capturedBasket.TotalPrice.ShouldBe(expectedTotal);

        _mockSender.Verify(s => s.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _mockRepository.Verify(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyItemsList_ShouldCreateEmptyBasket()
    {
        // Arrange
        const string userName = "testuser";
        
        var shoppingCartDto = new ShoppingCartDto
        {
            UserName = userName,
            Items = new List<ShoppingCartItemDto>()
        };

        var command = new CreateBasketCommand(shoppingCartDto);

        ShoppingCartEntity? capturedBasket = null;
        _mockRepository.Setup(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()))
                      .Callback<ShoppingCartEntity, CancellationToken>((basket, _) => capturedBasket = basket)
                      .ReturnsAsync((ShoppingCartEntity basket, CancellationToken _) => basket);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        capturedBasket.ShouldNotBeNull();
        capturedBasket!.Items.ShouldBeEmpty();
        capturedBasket!.UserName.ShouldBe(userName);
        capturedBasket!.TotalPrice.ShouldBe(0);

        _mockSender.Verify(s => s.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockRepository.Verify(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryFails_ShouldPropagateException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const string userName = "testuser";
        
        var product = ProductTestDataBuilder.New().WithId(productId).Build();

        var shoppingCartDto = new ShoppingCartDto
        {
            UserName = userName,
            Items = new List<ShoppingCartItemDto>
            {
                new() { ProductId = productId, Quantity = 1, Color = "Red" }
            }
        };

        var command = new CreateBasketCommand(shoppingCartDto);

        _mockSender.Setup(s => s.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new GetProductByIdResult(new ProductDto
                  {
                      Id = product.Id,
                      Name = product.Name,
                      Description = product.Description,
                      ImageFile = product.ImageFile,
                      Price = product.Price,
                      Category = product.Category
                  }));

        _mockRepository.Setup(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new InvalidOperationException("Repository error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        exception.Message.ShouldBe("Repository error");
    }
}