using Eshop.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EShop.Shared.DataSource.Extensions;

/// <summary>
/// Provides extension methods for <see cref="DbContext"/>.
/// </summary>
public static class DbContextExtension
{
    /// <summary>
    /// Attempts to find an entity with the specified primary key values.
    /// Throws a <see cref="NotFoundException"/> if the entity is not found.
    /// </summary>
    /// <typeparam name="T">The type of the entity to find.</typeparam>
    /// <param name="context">The <see cref="DbContext"/> instance.</param>
    /// <param name="keyValues">An array of primary key values for the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The found entity of type <typeparamref name="T"/>.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when an entity with the specified primary key values is not found.
    /// </exception>
    public static async Task<T> FindOrThrowAsync<T>(
        this DbContext context,
        object[] keyValues,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        var entity = await context.Set<T>().FindAsync(keyValues, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, string.Join(", ", keyValues));
        }

        return entity;
    }
}