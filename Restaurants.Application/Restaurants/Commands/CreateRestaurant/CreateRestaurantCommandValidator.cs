using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories =
    [
        "Italian",
        "Mexican",
        "Japanese",
        "American",
        "Indian",
        "Asian"
    ];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name).Length(3, 100);
        // RuleFor(dto => dto.Description).NotEmpty().WithMessage("Description is required.");
        // RuleFor(dto => dto.Category).NotEmpty().WithMessage("Insert a valid category.");
        // RuleFor(dto => dto.Category)
        //     .Custom(
        //         (value, context) =>
        //         {
        //             var isValidCategory = validCategories.Contains(value);
        //             if (!isValidCategory)
        //             {
        //                 context.AddFailure(
        //                     "Category",
        //                     "Invalid category. Please choose from the valid categories"
        //                 );
        //             }
        //         }
        //     );
        RuleFor(dto => dto.Category)
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address.");
        RuleFor(dto => dto.ContactNumber)
            .Matches(@"^\d{2}-\d{3}-\d{3}-\d{3}$")
            .WithMessage("Please provide a valid phoine number (XX-XXX-XXX-XXX).");
        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX).");
    }
}