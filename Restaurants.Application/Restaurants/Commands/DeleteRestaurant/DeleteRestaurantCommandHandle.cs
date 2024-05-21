using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandle(
    ILogger<DeleteRestaurantCommandHandle> logger,
    IRestaurantsRepository _restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService
) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Deleting restaurant with id: {RestaurantId} with {@UpdasteRestaurant}",
            request.Id,
            request
        );
        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperations.Delete))
        {
            throw new ForbiddenException(
                $"User not authorized to delete restaurant with {request.Id} id"
            );
        }
        await _restaurantsRepository.DeleteRestaurantAsync(restaurant!);
    }
}
