using EShop.Shared.Contracts.CQRS;
using MediatR;
using Moq;

namespace EShop.TestHelpers.Mocks;

public static class MockMediatR
{
    public static Mock<IMediator> CreateMediator()
    {
        return new Mock<IMediator>();
    }

    public static Mock<ICommandHandler<TCommand, TResponse>> CreateCommandHandler<TCommand, TResponse>()
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
    {
        return new Mock<ICommandHandler<TCommand, TResponse>>();
    }

    public static Mock<IQueryHandler<TQuery, TResponse>> CreateQueryHandler<TQuery, TResponse>()
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
        return new Mock<IQueryHandler<TQuery, TResponse>>();
    }

    public static Mock<IRequestHandler<TRequest, TResponse>> CreateRequestHandler<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return new Mock<IRequestHandler<TRequest, TResponse>>();
    }

    public static void SetupCommandHandler<TCommand, TResponse>(
        this Mock<IMediator> mediatorMock,
        TResponse response)
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(response);
    }

    public static void SetupQueryHandler<TQuery, TResponse>(
        this Mock<IMediator> mediatorMock,
        TResponse response)
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<TQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(response);
    }

    public static void VerifyCommandSent<TCommand>(
        this Mock<IMediator> mediatorMock,
        Times times)
        where TCommand : class
    {
        mediatorMock.Verify(
            m => m.Send(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()),
            times);
    }

    public static void VerifyQuerySent<TQuery>(
        this Mock<IMediator> mediatorMock,
        Times times)
        where TQuery : class
    {
        mediatorMock.Verify(
            m => m.Send(It.IsAny<TQuery>(), It.IsAny<CancellationToken>()),
            times);
    }
}