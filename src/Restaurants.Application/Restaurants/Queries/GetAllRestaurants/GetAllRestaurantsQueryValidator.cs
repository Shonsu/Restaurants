using System.Globalization;
using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowedPageSizes = [5, 10, 15, 30];
    private string[] allowedSortByColumns =
    [
        nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Description),
        nameof(RestaurantDto.Category)
    ];

    public GetAllRestaurantsQueryValidator()
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

        RuleFor(query => query.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize)
            .Must(value => allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(',', allowedPageSizes)}]");
        RuleFor(r => r.SortBy)
            .Must(value => allowedSortByColumns.Contains(value))
            .When(r => r.SortBy is not null)
            .WithMessage(
                $"Sort is optional, or by must be one of [{string.Join(',', allowedSortByColumns)}]"
            );
    }
}
