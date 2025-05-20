using System.Linq.Expressions;
using Eshop.Shared.Exceptions;
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
    /// Attempts to find a single entity matching the specified predicate.
    /// Throws a <see cref="NotFoundException"/> if no match is found.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="dbSet">The <see cref="DbSet{T}"/> to query.</param>
    /// <param name="predicate">A LINQ expression to filter the entity.</param>
    /// <param name="asNoTracking">Whether to query with no-tracking behavior.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The found entity of type <typeparamref name="T"/>.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no entity matches the specified predicate.
    /// </exception>
    public static async Task<T> SingleDefaultOrThrowAsync<T>(
        this DbSet<T> dbSet,
        Expression<Func<T, bool>> predicate,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        var query = asNoTracking ? dbSet.AsNoTracking() : dbSet;

        var entity = await query.SingleOrDefaultAsync(predicate, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, predicate.ToString());
        }

        return entity;
    }
}