using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository _restaurantsRepository
) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(
        CreateRestaurantCommand request,
        CancellationToken cancellationToken
    )
    {
        var restaurant = mapper.Map<Restaurant>(request);
        int id = await _restaurantsRepository.CreateRestaurantAsync(restaurant);
        logger.LogInformation("Create a new restaurant {@Restaurant}.", request);
        return id;
    }
}
