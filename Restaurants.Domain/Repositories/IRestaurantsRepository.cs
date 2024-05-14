using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<int> CreateRestaurantAsync(Restaurant restaurant);
    Task DeleteRestaurantAsync(Restaurant restaurant);
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int restaurantId);
    Task<bool> RestaurantExistAsync(int restaurantId);
}
