using EShop.Shared.DataSource.Extensions;
using EShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shared.DataSource.Interceptors;

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
            var isNewOrUpdated = entry.State == EntityState.Added && entry.State == EntityState.Modified;
            if (!isNewOrUpdated && !entry.HasChangedOwnedEntities()) continue;
            
            entry.Entity.UpdatedBy = "system";
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}