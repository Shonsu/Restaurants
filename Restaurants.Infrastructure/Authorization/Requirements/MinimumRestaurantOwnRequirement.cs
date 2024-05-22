using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    public class MinimumRestaurantOwnRequirement(int minimumNumberOfRestaurantCreated) : IAuthorizationRequirement
    {
        public int MinimumRestaurantCreated { get; } = minimumNumberOfRestaurantCreated;
    }
}
