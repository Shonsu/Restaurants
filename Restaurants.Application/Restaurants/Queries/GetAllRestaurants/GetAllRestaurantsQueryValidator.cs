using System.Globalization;
using FluentValidation;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowedPageSizes = [5, 10, 15, 30];

    public GetAllRestaurantsQueryValidator()
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

        RuleFor(query => query.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize)
            .Must(value => allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(',', allowedPageSizes)}]");
    }
}
