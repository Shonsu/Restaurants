using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandle(
    ILogger<DeleteRestaurantCommandHandle> logger,
    IRestaurantsRepository _restaurantsRepository
) : IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(
        DeleteRestaurantCommand request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "Deleting restaurant with id: {RestaurantId} with {@UpdasteRestaurant}",
            request.Id,
            request
        );
        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
        {
            return false;
        }
        await _restaurantsRepository.DeleteRestaurantAsync(restaurant!);
        return true;
    }
}
