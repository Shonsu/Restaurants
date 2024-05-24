using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;

public class DeleteDishesForRestaurantCommandHandler(
    ILogger<DeleteDishesForRestaurantCommandHandler> logger,
    IRestaurantsRepository _restaurantsRepository,
    IDishesRepository _dishesRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService
) : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    public async Task Handle(
        DeleteDishesForRestaurantCommand request,
        CancellationToken cancellationToken
    )
    {
        logger.LogWarning(
            "Delete dishes for repository with id {RestaurantId}",
            request.RestaurantId
        );

        var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        // it generate separate delete sql query for each dish
        // restaurant.Dishes.Clear();
        // await _restaurantsRepository.SaveChangesAsync();

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperations.Delete))
        {
            throw new ForbiddenException(
                $"User not authorized to delete dish from restaurant with {restaurant.Id} id"
            );
        }
        //below method behave the same :: generate separate delete sql query for each dish
        await _dishesRepository.Delete(restaurant.Dishes);
    }
}
