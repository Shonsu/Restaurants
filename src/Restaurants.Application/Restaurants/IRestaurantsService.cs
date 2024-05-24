using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    // Task<IEnumerable<RestaurantDto>> GetAllRestaurants();
    // Task<RestaurantDto?> GetRestaurantById(int restaurantId);
    // Task<int> CreateRestaurant(CreateRestaurantDto createRestaurantDto);
    Task<bool> RestaurantExist(int restaurantId);
}
