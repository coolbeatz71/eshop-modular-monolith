using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Ordering;

public static class OrderingModule
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // services
        //     .AddApplicationServices().
        //     AddInfrastructureServices()
        //     .AddApiServices();

        return services;
    }

}


