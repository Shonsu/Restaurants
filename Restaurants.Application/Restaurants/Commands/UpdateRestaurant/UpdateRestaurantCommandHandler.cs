using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository _restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService
) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        Restaurant? restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperations.Update))
        {
            throw new ForbiddenException(
                $"User not authorized to update restaurant with {request.Id} id"
            );
        }

        mapper.Map(request, restaurant);
        // restaurant.Name = request.Name;
        // restaurant.Description = request.Description;
        // restaurant.HasDelivery = request.HasDelivery;
        await _restaurantsRepository.SaveChangesAsync();

        logger.LogInformation(
            "Update restaurant with id: {RestaurantId} with {@UpdateRestaurant}",
            request.Id,
            request
        );
    }
}
