using EShop.Basket.Domain.Basket.Entities;
using EShop.Basket.Domain.Basket.Repositories;
using EShop.Shared.Domain.Specifications;
using Moq;

namespace EShop.Basket.UnitTests.Mocks;

public static class MockBasketRepository
{
    public static Mock<IBasketRepository> Create()
    {
        var mockRepository = new Mock<IBasketRepository>();

        // Setup default behaviors
        mockRepository.Setup(r => r.SaveChangesAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(1);

        mockRepository.Setup(r => r.CreateBasket(It.IsAny<ShoppingCartEntity>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((ShoppingCartEntity basket, CancellationToken _) => basket);

        mockRepository.Setup(r => r.DeleteBasket(It.IsAny<Specification<ShoppingCartEntity>>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        return mockRepository;
    }

    public static void SetupGetBasket(
        this Mock<IBasketRepository> mockRepository,
        ShoppingCartEntity? basket)
    {
        if (basket == null)
        {
            mockRepository.Setup(r => r.GetBasket(
                    It.IsAny<Specification<ShoppingCartEntity>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Basket not found"));
        }
        else
        {
            mockRepository.Setup(r => r.GetBasket(
                    It.IsAny<Specification<ShoppingCartEntity>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(basket);
        }
    }

    public static void VerifyGetBasketCalled(
        this Mock<IBasketRepository> mockRepository,
        Times times)
    {
        mockRepository.Verify(r => r.GetBasket(
            It.IsAny<Specification<ShoppingCartEntity>>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()), times);
    }

    public static void VerifyCreateBasketCalled(
        this Mock<IBasketRepository> mockRepository,
        Times times)
    {
        mockRepository.Verify(r => r.CreateBasket(
            It.IsAny<ShoppingCartEntity>(),
            It.IsAny<CancellationToken>()), times);
    }

    public static void VerifyDeleteBasketCalled(
        this Mock<IBasketRepository> mockRepository,
        Times times)
    {
        mockRepository.Verify(r => r.DeleteBasket(
            It.IsAny<Specification<ShoppingCartEntity>>(),
            It.IsAny<CancellationToken>()), times);
    }

    public static void VerifySaveChangesCalled(
        this Mock<IBasketRepository> mockRepository,
        Times times)
    {
        mockRepository.Verify(r => r.SaveChangesAsync(
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), times);
    }
}