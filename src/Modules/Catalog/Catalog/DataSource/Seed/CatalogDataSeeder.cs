using Microsoft.EntityFrameworkCore;
using EShop.Shared.DataSource.Seed;

namespace EShop.Catalog.DataSource.Seed;

/// <summary>
/// Seeds initial catalog data into the database if no product records exist.
/// </summary>
/// <param name="dbContext">The catalog database context used for data access.</param>
public class CatalogDataSeeder(CatalogDbContext dbContext) : IDataSeeder
{
    /// <summary>
    /// Seeds initial product data asynchronously if the database is empty.
    /// </summary>
    /// <returns>A task representing the asynchronous seed operation.</returns>
    /// <remarks>
    /// This method checks whether any product records exist. If none are found, it inserts initial products and saves changes.
    /// </remarks>
    public async Task SeedAllAsync()
    {
        // only seed if no product records exist in the DB
        if (!await dbContext.Products.AnyAsync())
        {
            await dbContext.Products.AddRangeAsync(InitialData.Products);
            await dbContext.SaveChangesAsync();
        }
    }
}