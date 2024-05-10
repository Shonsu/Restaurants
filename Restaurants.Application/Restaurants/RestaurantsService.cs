using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(
    IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger
) : IRestaurantsService
{
    private readonly IRestaurantsRepository _restaurantsRepository = restaurantsRepository;

    public async Task<IEnumerable<Restaurant>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        return await _restaurantsRepository.GetAllAsync();
    }

    public async Task<Restaurant?> GetRestaurantById(int restaurantId)
    {
        logger.LogInformation($"Get restaurant with id: {restaurantId}");
        return await _restaurantsRepository.GetByIdAsync(restaurantId);
    }

    public async Task<bool> RestaurantExist(int restaurantId)
    {
        return await _restaurantsRepository.RestaurantExistAsync(restaurantId);
    }
}
