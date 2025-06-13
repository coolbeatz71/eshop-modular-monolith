using EShop.Basket.DataSource;
using EShop.Basket.Domain.Basket.Dtos;
using EShop.Basket.Domain.Basket.Entities;
using EShop.Catalog.Domain.Products.UseCases.GetProductById;
using EShop.Shared.CQRS;
using EShop.Shared.Exceptions;
using MediatR;

namespace EShop.Basket.Domain.Basket.UseCases.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart): ICommand<CreateBasketResults>;
public record CreateBasketResults(Guid Id);

public class CreateBasketHandler(BasketDbContext dbContext, ISender sender): 
    ICommandHandler<CreateBasketCommand, CreateBasketResults>
{
    public Task<CreateBasketResults> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<ShoppingCartEntity> CreateBasket(
        ShoppingCartDto shoppingCartDto, 
        CancellationToken cancellationToken
    )
    {
        // Create new basket
        var basket = ShoppingCartEntity.Create(
            Guid.NewGuid(), 
            shoppingCartDto.UserName
        );

        foreach (var item in shoppingCartDto.Items)
        {
            var productResult = await sender.Send(
                 new GetProductByIdQuery(item.ProductId.ToString()), cancellationToken
            );

            if (productResult is null)
            {
                throw new NotFoundException(nameof(ShoppingCartEntity), item.ProductId);
            }
            
            basket.AddItem(
                productId: item.ProductId,
                quantity: item.Quantity,
                color: item.Color,
                price: productResult.Product.Price,
                productName: productResult.Product.Name
            );
        }
        
        return basket;
    }
}