using MediatR;

namespace Eshop.Shared.CQRS;

public interface ICommand : ICommand<Unit> { }

public interface ICommand<out TResponse>: IRequest<TResponse> { }