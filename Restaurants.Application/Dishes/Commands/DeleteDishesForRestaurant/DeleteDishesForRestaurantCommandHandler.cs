using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;

public class DeleteDishesForRestaurantCommandHandler(
    ILogger<DeleteDishesForRestaurantCommandHandler> logger,
    IRestaurantsRepository _restaurantsRepository,
    IDishesRepository _dishesRepository
) : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning(
            "Delete dishes for repository with id {RestaurantId}",
            request.RestaurantId
        );

        var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if(restaurant==null){
            throw new NotFoundException(nameof(Restaurant),request.RestaurantId.ToString());
        }
        // it generate separate delete sql query for each dish
        // restaurant.Dishes.Clear();
        // await _restaurantsRepository.SaveChangesAsync();

        //below method behave the same :: generate separate delete sql query for each dish
        await _dishesRepository.Delete(restaurant.Dishes);

    }
}
