using System.Linq.Expressions;

namespace EShop.Shared.Domain.Specifications;
/// <summary>
/// Base class for defining specifications and composing them using logical operators.
/// </summary>
/// <typeparam name="T">The type the specification applies to.</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    /// <inheritdoc />
    public List<Expression<Func<T, object>>> Includes { get; } = [];
    
    /// <inheritdoc />
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <inheritdoc />
    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }
    
    /// <summary>
    /// Adds a navigation property expression to the specification's include list.
    /// </summary>
    /// <param name="includeExpression">
    /// An expression representing a navigation property to be eagerly loaded, 
    /// typically used by ORMs such as Entity Framework (e.g., <c>x => x.Items</c>).
    /// </param>
    /// <remarks>
    /// This method enables specifications to include related entities 
    /// when constructing queries, helping to prevent lazy-loading or N+1 issues.
    /// </remarks>
    /// <example>
    /// <code>
    /// AddInclude(cart => cart.Items);
    /// </code>
    /// </example>
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// Combines the current specification with another using logical AND.
    /// </summary>
    public Specification<T> And(Specification<T> other) => new AndSpecification<T>(this, other);

    /// <summary>
    /// Combines the current specification with another using logical OR.
    /// </summary>
    public Specification<T> Or(Specification<T> other) => new OrSpecification<T>(this, other);

    /// <summary>
    /// Inverts the current specification using logical NOT.
    /// </summary>
    public Specification<T> Not() => new NotSpecification<T>(this);

    /// <summary>
    /// Combines multiple specifications using logical AND.
    /// </summary>
    public static Specification<T> AndAll(params Specification<T>[] specifications)
    {
        if (specifications == null || specifications.Length == 0)
        {
            throw new ArgumentException("At least one specification is required.");
        }

        return specifications.Aggregate(
            (current, next) => new AndSpecification<T>(current, next)
        );
    }

    /// <summary>
    /// Combines multiple specifications using logical OR.
    /// </summary>
    public static Specification<T> OrAll(params Specification<T>[] specifications)
    {
        if (specifications == null || specifications.Length == 0)
        {
            throw new ArgumentException("At least one specification is required.");
        }

        return specifications.Aggregate(
            (current, next) => new OrSpecification<T>(current, next)
        );
    }
}
