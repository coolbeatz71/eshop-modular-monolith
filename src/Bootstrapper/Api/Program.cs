var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!!");

// Configure the HTTP request pipelines/middlewares.
app.UseExceptionHandler(options => { });
app.UseAuthentication();
app.UseAuthorization();

// Configure middleware extensions for catalog, basket and ordering modules.
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
