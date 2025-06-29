using System.Linq.Expressions;

namespace EShop.Shared.Domain.Specifications;

/// <summary>
/// Represents a specification pattern that defines filtering criteria for objects of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object the specification applies to.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Converts the specification into a LINQ expression.
    /// </summary>
    /// <returns>An expression that represents the filtering criteria.</returns>
    Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Evaluates whether a given object satisfies the specification.
    /// </summary>
    /// <param name="entity">The object to evaluate.</param>
    /// <returns><c>true</c> if the object satisfies the specification; otherwise, <c>false</c>.</returns>
    bool IsSatisfiedBy(T entity);
    
    /// <summary>
    /// Gets the collection of navigation property expressions to be included 
    /// in the query result. These expressions are used by Object-Relational 
    /// Mappers (ORMs) like Entity Framework to eagerly load related entities.
    /// </summary>
    /// <remarks>
    /// This is typically used to avoid lazy-loading and N+1 query problems
    /// by specifying related data (e.g., child collections or references)
    /// that should be loaded along with the main entity.
    /// </remarks>
    /// <example>
    /// <code>
    /// specification.AddInclude(cart => cart.Items);
    /// </code>
    /// </example>
    List<Expression<Func<T, object>>> Includes { get; }
}
