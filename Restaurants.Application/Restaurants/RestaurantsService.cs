using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(
    IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger,
    IMapper mapper
) : IRestaurantsService
{
    private readonly IRestaurantsRepository _restaurantsRepository = restaurantsRepository;

    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await _restaurantsRepository.GetAllAsync();
        // var restaurantDtos = restaurants.Select(RestaurantDto.FromEntity);
        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        return restaurantDtos!;
    }

    public async Task<RestaurantDto?> GetRestaurantById(int restaurantId)
    {
        logger.LogInformation($"Get restaurant with id: {restaurantId}");
        var restaurant = await _restaurantsRepository.GetByIdAsync(restaurantId);
        //return RestaurantDto.FromEntity(restaurant);
        return mapper.Map<RestaurantDto>(restaurant);
    }

    public async Task<bool> RestaurantExist(int restaurantId)
    {
        return await _restaurantsRepository.RestaurantExistAsync(restaurantId);
    }
}
