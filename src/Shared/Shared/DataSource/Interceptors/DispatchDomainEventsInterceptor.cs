using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using EShop.Shared.Domain;

namespace EShop.Shared.DataSource.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator): SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result
    )
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? eventDataContext)
    {
        if(eventDataContext == null)  return;
        
        // get aggregates list
        var aggregates = eventDataContext.ChangeTracker
            .Entries<IAggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity);
        
        // get domainEvents from aggregates
        var enumerable = aggregates.ToList();
        var domainEvents = enumerable
            .SelectMany(x => x.DomainEvents)
            .ToList();
        
        // clear domain events
        enumerable.ToList().ForEach(x => x.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}