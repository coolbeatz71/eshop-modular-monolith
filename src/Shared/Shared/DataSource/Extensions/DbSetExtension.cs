using System.Linq.Expressions;
using EShop.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EShop.Shared.DataSource.Extensions;

/// <summary>
/// Provides extension methods for <see cref="DbSet{TEntity}"/>.
/// </summary>
public static class DbSetExtension
{
    /// <summary>
    /// Attempts to find an entity with the specified primary key values.
    /// Throws a <see cref="NotFoundException"/> if the entity is not found.
    /// </summary>
    /// <typeparam name="T">The type of the entity to find.</typeparam>
    /// <param name="dbSet">The <see cref="DbSet{T}"/> to query.</param>
    /// <param name="keyValues">An array of primary key values for the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The found entity of type <typeparamref name="T"/>.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when an entity with the specified primary key values is not found.
    /// </exception>
    public static async Task<T> FindOrThrowAsync<T>(
        this DbSet<T> dbSet,
        object[] keyValues,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        var context = dbSet.GetService<ICurrentDbContext>().Context;

        var entity = await context.Set<T>().FindAsync(keyValues, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, string.Join(", ", keyValues));
        }

        return entity;
    }
    
    /// <summary>
    /// Attempts to retrieve a single entity matching the specified predicate from the query.
    /// Throws a <see cref="NotFoundException"/> if no entity is found.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to query against.</param>
    /// <param name="predicate">A predicate to filter the entities.</param>
    /// <param name="asNoTracking">If true, the query will be executed with no tracking.</param>
    /// <param name="key">
    /// An optional key or label to include in the exception message if the entity is not found.
    /// If not provided, the predicate expression will be used.
    /// </param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The entity that matches the predicate.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no entity matching the predicate is found.
    /// </exception>
    /// <example>
    /// Example usage:
    /// <code>
    /// var product = await dbContext.Products
    ///     .SingleDefaultOrThrowAsync(p => p.Sku == sku, key: sku);
    /// </code>
    /// </example>
    public static async Task<T> SingleDefaultOrThrowAsync<T>(
        this IQueryable<T> query,
        Expression<Func<T, bool>> predicate,
        bool asNoTracking = false,
        string? key = null,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        var entity = await query.SingleOrDefaultAsync(predicate, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, key ?? predicate.ToString());
        }

        return entity;
    }

    /// <summary>
    /// Attempts to retrieve a single entity from the query.
    /// Throws a <see cref="NotFoundException"/> if no entity is found.
    /// Assumes that any filtering (e.g., via <c>.Where()</c>) is applied before calling this method.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to execute.</param>
    /// <param name="asNoTracking">If true, the query will be executed with no tracking.</param>
    /// <param name="key">
    /// An optional key or label to include in the exception message if the entity is not found.
    /// If not provided, the query expression will be used.
    /// </param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The single entity matching the query.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no entity is found.
    /// </exception>
    /// <example>
    /// Example usage:
    /// <code>
    /// var basket = await dbContext.Baskets
    ///     .Where(b => b.UserName == userName)
    ///     .SingleDefaultOrThrowAsync(key: userName);
    /// </code>
    /// </example>
    public static async Task<T> SingleDefaultOrThrowAsync<T>(
        this IQueryable<T> query,
        bool asNoTracking = false,
        string? key = null,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, key ?? query.ToString());
        }

        return entity;
    }
}