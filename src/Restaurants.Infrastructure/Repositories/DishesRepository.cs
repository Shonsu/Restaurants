using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure;

internal class DishesRepository(RestaurantsDBContext dbContext) : IDishesRepository
{
    public async Task<int> CreateDish(Dish dish)
    {
        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync();
        return dish.Id;
    }

    public async Task Delete(IEnumerable<Dish> dishes)
    {
        dbContext.RemoveRange(dishes);
        await dbContext.SaveChangesAsync();
    }

    // public async Task<IEnumerable<Dish>> GetDishesForRestaurantAsync(int restaurantId)
    // {
    //     List<Dish> dishes = await dbContext
    //         .Dishes.Where(d => d.RestaurantId == restaurantId)
    //         .ToListAsync();
    //     return dishes;
    // }
}
