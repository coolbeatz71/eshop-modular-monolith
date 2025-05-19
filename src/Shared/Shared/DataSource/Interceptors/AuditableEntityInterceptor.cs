using EShop.Shared.DataSource.Extensions;
using EShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EShop.Shared.DataSource.Interceptors;

public class AuditableEntityInterceptor: SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    /// <summary>
    /// Updates audit fields (<c>CreatedBy</c>, <c>CreatedAt</c>, <c>UpdatedBy</c>, <c>UpdatedAt</c>) 
    /// for entities tracked by the <see cref="DbContext"/> that implement <see cref="IEntity"/>.
    /// </summary>
    /// <param name="eventDataContext">
    /// The <see cref="DbContext"/> instance tracking the entity changes. If <c>null</c>, the method exits early.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method currently sets the <c>CreatedBy</c> and <c>UpdatedBy</c> fields to the hardcoded value "system".
    /// It detects newly added and modified entities, and sets their timestamps and audit metadata accordingly.
    /// </para>
    ///
    /// <para><b>TODO:</b> Enhance this method to retrieve the actual user who triggered the change (e.g., from an injected user context),
    /// or identify if the change originated from a background job, system process, or another automated source.</para>
    /// </remarks>
    private void UpdateEntities(DbContext? eventDataContext)
    {
        if(eventDataContext == null)  return;

        foreach (var entry in eventDataContext.ChangeTracker.Entries<IEntity>())
        {
            // the case of creation
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "system";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            
            // the case of edit/update
            var isNewOrUpdated = entry.State is EntityState.Added or EntityState.Modified;
            if (!isNewOrUpdated && !entry.HasChangedOwnedEntities()) continue;
            
            entry.Entity.UpdatedBy = "system";
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}