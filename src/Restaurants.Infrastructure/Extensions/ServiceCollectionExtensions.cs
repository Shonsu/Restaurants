using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Configuration;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Infrastructure.Storage;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        SqlConnectionStringBuilder conStrBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("RestaurantDb"));
        if (conStrBuilder["Server"].ToString()!.Contains("locahost"))
        {
            conStrBuilder.Password = configuration["DbPassword"];
        }
        //var connectionString = configuration.GetConnectionString("RestaurantDb");
        services.AddDbContext<RestaurantsDBContext>(options =>
            options.UseSqlServer(conStrBuilder.ConnectionString).EnableSensitiveDataLogging()
        );
        services
            .AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDBContext>();
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                PolicyNames.HasNationality,
                builder => builder.RequireClaim(AppClaimTypes.Nationality, "German", "Polish")
            )
            .AddPolicy(
                PolicyNames.AtLeast20,
                builder => builder.AddRequirements(new MinimumAgeRequirement(20))
            )
            .AddPolicy(
                PolicyNames.OwnerAtLeast2restaurant,
                builder => builder.AddRequirements(new MinimumRestaurantOwnRequirement(2))
            );

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, MinimumRestaurantOwnRequirementHandler>();
        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
    }
}
