using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // services
        //     .AddApplicationServices().
        //     AddInfrastructureServices()
        //     .AddApiServices();

        return services;
    }

}


