using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopAPI.Data;
using ShopAPI.Services;
using ShopAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

{
    var configuration = builder.Configuration;
    var services = builder.Services;

    services.AddControllers();
    
    services.AddEndpointsApiExplorer();
    
    services.AddSwaggerGen(o =>
    {
        o.SwaggerDoc("v1", new OpenApiInfo()
        {
            Version = "v1",
            Title = "Shop API",
            Description = "Shop API swagger documentation.",
        });

        o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        
        o.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
    
    services.AddDbContext<AppDbContext>(o => 
        o.UseInMemoryDatabase("ShopDatabase"));

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Authentication:JwtBearer:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]))
            };
        });

    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<INegotiationService, NegotiationService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop API v1");
        o.RoutePrefix = string.Empty;
    });
}

app.UseCors(x => x
    .SetIsOriginAllowed(origin => true)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
