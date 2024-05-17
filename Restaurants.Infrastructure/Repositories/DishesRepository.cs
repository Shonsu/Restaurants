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
}
