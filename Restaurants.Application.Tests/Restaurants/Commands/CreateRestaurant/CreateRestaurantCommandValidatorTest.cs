using FluentAssertions;
using FluentValidation.TestHelper;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTest
{
    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Category = "Asian",
            ContactEmail = "test@test.com",
            ContactNumber = "22-123-123-123",
            PostalCode = "22-123"
        };
        var validator = new CreateRestaurantCommandValidator();
        // act
        var validationResult = validator.TestValidate(command);
        // asserts
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Category = "Turkey",
            ContactEmail = "testtest.com",
            ContactNumber = "22-123-123-",
            PostalCode = "22-1213"
        };
        var validator = new CreateRestaurantCommandValidator();
        // act
        var validationResult = validator.TestValidate(command);
        // asserts
        validationResult.ShouldHaveValidationErrorFor(ve => ve.Name);
        validationResult.ShouldHaveValidationErrorFor(ve => ve.Category);
        validationResult.ShouldHaveValidationErrorFor(ve => ve.ContactEmail);
        validationResult.ShouldHaveValidationErrorFor(ve => ve.ContactNumber);
        validationResult.ShouldHaveValidationErrorFor(ve => ve.Name);
    }

    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    [InlineData("Asian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(
        string category
    )
    {
        // arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand() { Category = category };
        // act
        var validationResult = validator.TestValidate(command);
        // asserts
        validationResult.ShouldNotHaveValidationErrorFor(ve => ve.Category);
    }

    [Theory]
    [InlineData("1-123")]
    [InlineData("12-54")]
    [InlineData("321-12")]
    [InlineData("a1-123")]
    [InlineData("12-qwe")]
    [InlineData("qw-qwe")]
    public void Validator_ForinValidPostalCodes_ShouldNotHaveValidationErrorsForCategoryProperty(
        string postalCode
    )
    {
        // arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand() { PostalCode = postalCode };
        // act
        var validationResult = validator.TestValidate(command);
        // asserts
        validationResult.ShouldHaveValidationErrorFor(ve => ve.PostalCode);
    }
}
