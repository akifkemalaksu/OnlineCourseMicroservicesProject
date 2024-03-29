using FreeCourse.Gateway.DelegateHandlers;
using FreeCourse.Gateway.Models;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration["IdentityServerUrl"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();
});

builder.Services.Configure<TokenExchangeSettings>(builder.Configuration.GetSection("TokenExchangeSettings"));

builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();
builder.Services.AddOcelot().AddDelegatingHandler<TokenExchangeDelegateHandler>();

var app = builder.Build();

await app.UseOcelot();

app.Run();