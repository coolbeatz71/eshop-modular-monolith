using Eshop.Shared.CQRS;
using EShop.Shared.Domain;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace EShop.Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var firstError = validationResults
            .SelectMany(r => r.Errors)
            .FirstOrDefault(f => f != null);

        if (firstError is null)
        {
            return await next(cancellationToken);
        }

        return CreateErrorResponse(firstError.ErrorMessage);
    }

    private static TResponse CreateErrorResponse(string errorMessage)
    {
        var genericArg = GetGenericArgument(typeof(TResponse));
        var resultType = typeof(Response<>).MakeGenericType(genericArg);
        var failureMethod = GetFailureMethod(resultType);
        var response = InvokeFailureMethod(failureMethod, errorMessage);

        return (TResponse)response;
    }
    
    private static Type GetGenericArgument(Type responseType) =>
        responseType.GetGenericArguments().FirstOrDefault()
        ?? throw new InvalidOperationException("TResponse must be a generic type.");

    private static MethodInfo GetFailureMethod(Type resultType) =>
        resultType.GetMethod("Failure", BindingFlags.Public | BindingFlags.Static)
        ?? throw new InvalidOperationException($"Static Failure method not found on {resultType.Name}");

    private static object InvokeFailureMethod(MethodInfo method, string errorMessage) =>
        method.Invoke(null, [errorMessage])
        ?? throw new InvalidOperationException("Failure method returned null.");
}