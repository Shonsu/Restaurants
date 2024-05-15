using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(
    ILogger<GetRestaurantByIdQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository _restaurantsRepository
) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
{
    public async Task<RestaurantDto?> Handle(
        GetRestaurantByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Get restaurant with id: {RestaurantId}", request.Id);
        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);
        //return RestaurantDto.FromEntity(restaurant);
        var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);
        return restaurantDto;
    }
}
