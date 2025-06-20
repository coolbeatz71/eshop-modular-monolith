using EShop.Basket.Domain.Basket.Repositories;
using EShop.Shared.CQRS;

namespace EShop.Basket.Domain.Basket.UseCases.DeleteBasket;

public record DeleteBasketCommand(string UserName): ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteBasket(command.UserName, cancellationToken);
        
        return new DeleteBasketResult(true);
    }
}