using Microsoft.EntityFrameworkCore;
using EShop.Shared.DataSource.Seed;

namespace EShop.Catalog.DataSource.Seed;

public class CatalogDataSeeder(CatalogDbContext dbContext) : IDataSeeder
{
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