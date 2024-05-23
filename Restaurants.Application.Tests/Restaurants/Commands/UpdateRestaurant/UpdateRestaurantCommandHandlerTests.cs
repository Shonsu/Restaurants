using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
 
    [Fact]
    public async void Handle_WithValidReqest_ShouldUpdateRestaurant()
    {
        // arrange
        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        var mapperConfiguration = new MapperConfiguration(cfg =>
            cfg.AddProfile<RestaurantsProfile>()
        );
        var mapper = mapperConfiguration.CreateMapper();
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        var request = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Name",
            Description = "Desc",
            HasDelivery = true
        };
        var restaurantToUpdate = new Restaurant();
        restaurantsRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(restaurantToUpdate);
        restaurantsRepositoryMock.Setup(r => r.SaveChangesAsync()).Verifiable();

        var restaurantAuthorizationService = new Mock<IRestaurantAuthorizationService>();
        restaurantAuthorizationService
            .Setup(auth => auth.Authorize(restaurantToUpdate, ResourceOperations.Update))
            .Returns(true);

        var commandHandler = new UpdateRestaurantCommandHandler(
            loggerMock.Object,
            mapper,
            restaurantsRepositoryMock.Object,
            restaurantAuthorizationService.Object
        );

        // act
        await commandHandler.Handle(request, CancellationToken.None);

        // asserts
        restaurantToUpdate.Should().NotBeNull();
        restaurantToUpdate.Id.Should().Be(request.Id);
        restaurantToUpdate.Name.Should().Be(request.Name);
        restaurantToUpdate.Description.Should().Be(request.Description);
        restaurantToUpdate.HasDelivery.Should().BeTrue();
        restaurantsRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        //mapper.Verify(m => m.Map(request, restaurantToUpdate), Times.Once);
    }

    [Fact]
    public async void UpdateRestaurant_WhenUpdatedRestaurantNotExist_ThrowNotFoundException()
    {
        // arrange
        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var request = new UpdateRestaurantCommand() { Id = 1 };
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Restaurant?)null);
        var restaurantAuthorizationService = new Mock<IRestaurantAuthorizationService>();

        var commandHandler = new UpdateRestaurantCommandHandler(
            loggerMock.Object,
            mapperMock.Object,
            restaurantsRepositoryMock.Object,
            restaurantAuthorizationService.Object
        );

        // act
        Func<Task> action = async () =>
            await commandHandler.Handle(request, CancellationToken.None);

        // asserts
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Restaurant with id: 1 doesn't exist.");
    }

    [Fact]
    public async void UpdateRestaurant_WhenUserIsNotAuthorized_ThrowForbiddenException()
    {
        // arrange
        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var request = new UpdateRestaurantCommand() { Id = 1 };
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Restaurant());
        var restaurantToUpdate = new Restaurant();

        var restaurantAuthorizationService = new Mock<IRestaurantAuthorizationService>();
        restaurantAuthorizationService
            .Setup(auth => auth.Authorize(restaurantToUpdate, ResourceOperations.Update))
            .Returns(false);
        var commandHandler = new UpdateRestaurantCommandHandler(
            loggerMock.Object,
            mapperMock.Object,
            restaurantsRepositoryMock.Object,
            restaurantAuthorizationService.Object
        );

        // act
        Func<Task> action = async () =>
            await commandHandler.Handle(request, CancellationToken.None);

        // asserts
        await action
            .Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage("User not authorized to update restaurant with 1 id");
    }
}
