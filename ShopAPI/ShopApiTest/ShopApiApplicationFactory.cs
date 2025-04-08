using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ShopAPI.Data;
using ShopAPI.Services;
using ShopAPI.Services.Interfaces;

namespace ShopApiTest;

public class ShopApiApplicationFactory : WebApplicationFactory<Program>
{
    public IConfiguration Configuration { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<ShopApiApplicationFactory>()
                .Build();

            config.AddConfiguration(Configuration);
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<INegotiationService, NegotiationService>();
            
            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "TestScheme", options => { });
            
            services.AddAuthorization(o =>
            {
                o.AddPolicy("RequireAuth", policy => policy.RequireAuthenticatedUser());
            });
                
            services.AddDbContext<AppDbContext>(o => 
                o.UseInMemoryDatabase("ShopDatabase"));
        });
    }
}