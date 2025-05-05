var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddIdentityModule(builder.Configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!!");

// Configure the HTTP request pipeline.

app.Run();
