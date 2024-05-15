using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(r => r.Name).Length(3, 100);
        // RuleFor(r => r.Description).Length(3, 200);
        // RuleFor(r => r.HasDelivery).NotNull().NotEmpty();
    }
}
