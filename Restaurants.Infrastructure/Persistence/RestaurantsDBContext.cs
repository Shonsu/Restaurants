using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure;

internal class RestaurantsDBContext(DbContextOptions<RestaurantsDBContext> options)
    : IdentityDbContext<User>(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; }
    internal DbSet<Dish> Dishes { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(
    //         "Server=localhost,1433;Database=RestaurantDB;User Id=restaurantLogin;Password=restaurant@123;TrustServerCertificate=True;" //MultiSubnetFailover=True;
    //     );
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.Entity<Restaurant>().OwnsOne(r => r.Address);
        // modelBuilder
        //     .Entity<Restaurant>()
        //     .HasMany(r => r.Dishes)
        //     .WithOne()
        //     .HasForeignKey(d => d.RestaurantId);
        modelBuilder.Entity<Restaurant>(mb =>
        {
            mb.OwnsOne(r => r.Address);
            mb.HasMany(r => r.Dishes).WithOne().HasForeignKey(d => d.RestaurantId);
        });
    }
}
