using EShop.Basket.Domain.Basket.Repositories;
using EShop.Shared.CQRS;

namespace EShop.Basket.Domain.Basket.UseCases.DeleteBasket;

/// <summary>
/// Represents a command to delete a basket associated with a specific user.
/// </summary>
/// <param name="UserName">The username of the user whose basket is to be deleted.</param>
public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

/// <summary>
/// Represents the result of a basket deletion operation.
/// </summary>
/// <param name="IsSuccess">Indicates whether the basket was successfully deleted.</param>
public record DeleteBasketResult(bool IsSuccess);

/// <summary>
/// Handles the <see cref="DeleteBasketCommand"/> to delete a user's basket.
/// </summary>
public class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    /// <summary>
    /// Executes the logic to delete the basket for the specified user.
    /// </summary>
    /// <param name="command">The command containing the username of the basket owner.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="DeleteBasketResult"/> indicating the success status of the deletion operation.
    /// </returns>
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteBasket(command.UserName, cancellationToken);
        
        return new DeleteBasketResult(true);
    }
}