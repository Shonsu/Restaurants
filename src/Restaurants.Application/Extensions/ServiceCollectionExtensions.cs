using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Users;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddScoped<IUserContext, UserContext>();
        var appliactionAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appliactionAssembly));
        services.AddAutoMapper(appliactionAssembly);
        services.AddValidatorsFromAssembly(appliactionAssembly).AddFluentValidationAutoValidation();
        services.AddHttpContextAccessor();
    }
}
