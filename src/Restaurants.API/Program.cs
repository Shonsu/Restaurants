using Restaurants.API.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.


    builder.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetService<IRestaurantSeeder>();
    if (seeder != null)
    {
        await seeder.Seed();
    }

    // Configure the HTTP request pipeline.
    app.UseMiddleware<RequestTimeLoggingMiddleware>();
    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseSerilogRequestLogging();
    app.MapGroup("api/identity").WithTags("Identity").MapIdentityApi<User>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{

    Log.Fatal(ex, "application startup failed");
}
finally
{
    Log.CloseAndFlush();
}


public partial class Program { }
