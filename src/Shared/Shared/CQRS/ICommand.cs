using MediatR;

namespace Eshop.Shared.CQRS;

/// <summary>
/// Represents a command that does not return a result.
/// This is a marker interface inheriting from <see cref="ICommand{Unit}"/>.
/// </summary>
public interface ICommand : ICommand<Unit>;

/// <summary>
/// Represents a command with a response of type <typeparamref name="TResponse"/>.
/// Extends <see cref="IRequest{TResponse}"/> from MediatR.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned when the command is handled.</typeparam>
public interface ICommand<out TResult> : IRequest<TResult>;