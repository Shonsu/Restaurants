using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler(ILogger<UploadRestaurantLogoCommandHandler> logger, IRestaurantsRepository restaurantsRepository, IRestaurantAuthorizationService restaurantAuthorizationService, IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Uploading restaurant logo for id: {RestaurantId}.", request.RestaurantId);

        Restaurant? restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperations.Update))
        {
            throw new ForbiddenException(
                $"User not authorized to update restaurant with {request.RestaurantId} id"
            );
        }

        string logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);
        restaurant.LogoUrl = logoUrl;

        await restaurantsRepository.SaveChangesAsync();
    }
}