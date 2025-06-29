using Microsoft.EntityFrameworkCore;

namespace EShop.Shared.Domain.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> inputQuery,
        ISpecification<T> specification,
        bool asNoTracking = false
    ) where T : class
    {
        var query = specification.Includes
            .Aggregate(inputQuery, (current, include) => current.Include(include));
        
        // Apply tracking behavior
        if (asNoTracking) query = query.AsNoTracking();
        
        // Apply filter
        return query.Where(specification.ToExpression());
    }
}