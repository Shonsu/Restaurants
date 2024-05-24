using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class MinimumRestaurantOwnRequirementHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_HasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // arrange
        var numberUserRestaurants = 2;
        var minRequiredRestaurantNumber = 2;
        var loggerMock = new Mock<ILogger<MinimumRestaurantOwnRequirementHandler>>();
        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("currentUserId", "test@test.com", [], null, null);
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock
            .Setup(rr => rr.CountUserRestaurants(currentUser.Id))
            .ReturnsAsync(numberUserRestaurants);
        var requirement = new MinimumRestaurantOwnRequirement(minRequiredRestaurantNumber);
        var requirementHandler = new MinimumRestaurantOwnRequirementHandler(
            loggerMock.Object,
            userContextMock.Object,
            restaurantsRepositoryMock.Object
        );
        var authorizationHandlerContext = new AuthorizationHandlerContext(
            [requirement],
            null,
            null
        );
        // act
        await requirementHandler.HandleAsync(authorizationHandlerContext);

        //assert
        authorizationHandlerContext.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_HasNotCreatedMultipleRestaurants_ShouldFail()
    {
        // arrange
        var numberRestaurants = 1;
        var minRequiredRestaurantNumber = 2;
        var loggerMock = new Mock<ILogger<MinimumRestaurantOwnRequirementHandler>>();
        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("currentUserId", "test@test.com", [], null, null);
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock
            .Setup(rr => rr.CountUserRestaurants(currentUser.Id))
            .ReturnsAsync(numberRestaurants);
        var requirement = new MinimumRestaurantOwnRequirement(minRequiredRestaurantNumber);
        var requirementHandler = new MinimumRestaurantOwnRequirementHandler(
            loggerMock.Object,
            userContextMock.Object,
            restaurantsRepositoryMock.Object
        );
        var authorizationHandlerContext = new AuthorizationHandlerContext(
            [requirement],
            null,
            null
        );
        // act
        await requirementHandler.HandleAsync(authorizationHandlerContext);

        //assert
        authorizationHandlerContext.HasFailed.Should().BeTrue();
        authorizationHandlerContext.HasSucceeded.Should().BeFalse();
    }
}
