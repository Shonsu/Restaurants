namespace Restaurants.Application.Restaurants.Dtos;

public class UpdateRestaurantDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool HasDelivery { get; set; }
}
