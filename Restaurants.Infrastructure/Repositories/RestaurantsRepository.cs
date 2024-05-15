using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure;

internal class RestaurantsRepository(RestaurantsDBContext dbContext) : IRestaurantsRepository
{
    public async Task<int> CreateRestaurantAsync(Restaurant restaurant)
    {
        await dbContext.Restaurants.AddAsync(restaurant);
        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task DeleteRestaurantAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int restaurantId)
    {
        return await dbContext
            .Restaurants.AsNoTracking()
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == restaurantId);
    }

    public async Task<bool> RestaurantExistAsync(int restaurantId)
    {
        return await dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId);
    }

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();

    public async Task<bool> UpdateRestaurantAsync(int id, Object restaurant)
    {
        Restaurant? restaurantToUpdate = await dbContext.Restaurants.FirstOrDefaultAsync(r =>
            r.Id == id
        );
        if (restaurantToUpdate == null)
        {
            return false;
        }
        dbContext.Entry(restaurantToUpdate).CurrentValues.SetValues(restaurant);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
