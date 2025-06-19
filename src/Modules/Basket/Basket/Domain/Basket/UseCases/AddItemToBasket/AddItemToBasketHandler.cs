using EShop.Basket.Domain.Basket.Dtos;
using EShop.Basket.Domain.Basket.Repositories;
using EShop.Catalog.Domain.Products.UseCases.GetProductById;
using EShop.Shared.CQRS;
using MediatR;

namespace EShop.Basket.Domain.Basket.UseCases.AddItemToBasket;

public record AddItemToBasketCommand(
    string UserName, 
    ShoppingCartItemDto ShoppingCartItem
): ICommand<AddItemToBasketResult>;
public record AddItemToBasketResult(Guid Id);

public class AddItemToBasketHandler(IBasketRepository repository, ISender sender)
    : ICommandHandler<AddItemToBasketCommand, AddItemToBasketResult>
{
    public async Task<AddItemToBasketResult> Handle(AddItemToBasketCommand command, CancellationToken cancellationToken)
    {
        // Add shopping cart item into shopping cart
        var shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);
        
        // Before AddItem into ShoppingCart, we should call Catalog Module GetProductByIdQuery method
        // Get latest product information and set Price and ProductName when adding item into Basket
        var productQuery = new GetProductByIdQuery(command.ShoppingCartItem.ProductId.ToString());
        var result = await sender.Send(productQuery, cancellationToken);
        
        shoppingCart.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            command.ShoppingCartItem.Color,
            result.Product.Price,
            result.Product.Name
        );
        
        await repository.SaveChangesAsync(command.UserName, cancellationToken);
        
        return new AddItemToBasketResult(shoppingCart.Id);
    }
}