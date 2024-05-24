using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<int> CreateDish(Dish dish);
    Task Delete(IEnumerable<Dish> dishes);
    // Task<IEnumerable<Dish>> GetDishesForRestaurantAsync(int restaurantId);
}
