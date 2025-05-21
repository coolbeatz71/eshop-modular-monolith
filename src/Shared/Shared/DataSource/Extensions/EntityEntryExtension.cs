using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EShop.Shared.DataSource.Extensions;

/// <summary>
/// Provides extension methods for <see cref="EntityEntry"/> to assist with tracking owned entity changes.
/// </summary>
public static class EntityEntryExtension
{
    /// <summary>
    /// Determines whether the <see cref="EntityEntry"/> has any related owned entities that are newly added or modified.
    /// </summary>
    /// <param name="entry">The entity entry to inspect.</param>
    /// <returns><c>true</c> if any owned entities have been added or modified; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var entry = dbContext.Entry(someEntity);
    /// bool hasChanges = entry.HasChangedOwnedEntities();
    /// if (hasChanges)
    /// {
    ///     // Perform logic when owned entities have changed
    /// }
    /// </code>
    /// </example>
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified)
        );
}
