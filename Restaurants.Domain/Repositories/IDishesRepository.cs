using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<int> CreateDish(Dish dish);
    // Task<IEnumerable<Dish>> GetDishesForRestaurantAsync(int restaurantId);
}
