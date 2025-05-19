var builder = WebApplication.CreateBuilder(args);

// Load environments variables from .env file
DotNetEnv.Env.Load();
DotNetEnv.Env.TraversePath().Load();

// Add services to the container.
builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!!");

// Configure middleware extensions for catalog, basket and ordering modules.
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
