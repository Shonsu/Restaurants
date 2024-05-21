using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;



public class RestaurantAuthorizationService(
    ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext
) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperations resourceOperations)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation(
            "Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
            user.Email,
            resourceOperations,
            restaurant.Name
        );

        if (
            resourceOperations == ResourceOperations.Read
            || resourceOperations == ResourceOperations.Create
        )
        {
            logger.LogInformation("Create/read operation - successful authorization");
            return true;
        }

        if (resourceOperations == ResourceOperations.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;
        }

        if (
            (
                resourceOperations == ResourceOperations.Delete
                || resourceOperations == ResourceOperations.Update
            )
            && user.Id == restaurant.OwnerId
        )
        {
            logger.LogInformation("Restaurant owner - successful authorization");
            return true;
        }
        return false;
    }
}
