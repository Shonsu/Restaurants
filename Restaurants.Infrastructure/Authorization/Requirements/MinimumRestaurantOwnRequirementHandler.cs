using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class MinimumRestaurantOwnRequirementHandler(
    ILogger<MinimumRestaurantOwnRequirementHandler> logger,
    IUserContext userContext,
    IRestaurantsRepository restaurantsRepository
) : AuthorizationHandler<MinimumRestaurantOwnRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumRestaurantOwnRequirement requirement
    )
    {
        var user = userContext.GetCurrentUser()!;
        int restaurantNumber = await restaurantsRepository.CountUserRestaurants(user.Id);
        logger.LogInformation(
            "{UserEmail} with {RestaurantNumber} restaurants - handling MinimumRestaurantOwn",
            user.Email,
            restaurantNumber
        );
        if (requirement.MinimumRestaurantCreated <= restaurantNumber)
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            logger.LogInformation("Authorization failed");
            context.Fail();
        }
    }
}
