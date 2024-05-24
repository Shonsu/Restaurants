using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(
    ILogger<CreateDishCommandHandler> logger,
    IRestaurantsRepository _restaurantsRepository,
    IDishesRepository _dishesRepository,
    IMapper mapper,
    IRestaurantAuthorizationService restaurantAuthorizationService
) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Create new dish: {@DishRequest}.", request);
        var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        var dish = mapper.Map<Dish>(request);

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperations.Update))
        {
            throw new ForbiddenException(
                $"User not authorized to create dish from restaurant with {restaurant.Id} id"
            );
        }
        await _dishesRepository.CreateDish(dish);
        return dish.Id;
    }
}
