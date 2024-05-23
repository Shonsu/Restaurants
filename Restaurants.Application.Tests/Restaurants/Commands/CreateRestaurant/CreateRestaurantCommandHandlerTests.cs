using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact]
    public async void Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var createRestaurantCommand = new CreateRestaurantCommand();
        var restaurant = new Restaurant();
        mapperMock.Setup(m => m.Map<Restaurant>(createRestaurantCommand)).Returns(restaurant);

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock
            .Setup(s => s.CreateRestaurantAsync(It.IsAny<Restaurant>()))
            .ReturnsAsync(1);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "email@com.com", [], null, null);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateRestaurantCommandHandler(
            loggerMock.Object,
            mapperMock.Object,
            restaurantsRepositoryMock.Object,
            userContextMock.Object
        );

        //act
        int result = await commandHandler.Handle(createRestaurantCommand, CancellationToken.None);

        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner-id");
        restaurantsRepositoryMock.Verify(r => r.CreateRestaurantAsync(restaurant), Times.Once);
    }
}
