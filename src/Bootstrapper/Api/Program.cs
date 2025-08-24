using Carter;
using EShop.Shared.Exceptions.Handlers;
using EShop.Shared.Extensions;
using Keycloak.AuthServices.Authentication;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Load environments variables from .env file
DotNetEnv.Env.Load();
DotNetEnv.Env.TraversePath().Load();

// Add services to the container.
// Register Carter and MediatR Assemblies
var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithAssemblies(
    catalogAssembly, 
    basketAssembly
);

builder.Services.AddMediatRWithAssemblies(
    catalogAssembly, 
    basketAssembly
);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();


app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(_ => { });
app.UseAuthentication();
app.UseAuthorization();

// Configure middleware extensions for catalog, basket and ordering modules.
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();

public partial class Program { }
