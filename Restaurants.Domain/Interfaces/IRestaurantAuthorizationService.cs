using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Interfaces;

public interface IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperations resourceOperations);
}
