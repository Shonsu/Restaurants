using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationService;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationService = new Mock<IRestaurantAuthorizationService>();
        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _mapperMock.Object,
            _restaurantsRepositoryMock.Object,
            _restaurantAuthorizationService.Object
        );
    }

    [Fact]
    public async void Handle_WithValidReqest_ShouldUpdateRestaurant()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "Name",
            Description = "Desc",
            HasDelivery = true
        };

        var restaurantToUpdate = new Restaurant()
        {
            Id = restaurantId,
            Name = "test",
            Description = "some old description",
            HasDelivery = false
        };

        _restaurantsRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurantToUpdate);

        _restaurantAuthorizationService
            .Setup(auth => auth.Authorize(restaurantToUpdate, ResourceOperations.Update))
            .Returns(true);

        // act
        await _handler.Handle(command, CancellationToken.None);

        // asserts
        _restaurantsRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurantToUpdate), Times.Once);
    }

    [Fact]
    public async void Handle_WhenUpdatedRestaurantNotExist_ThrowNotFoundException()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand() { Id = restaurantId };

        _restaurantsRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null);

        // act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);
        // asserts
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} doesn't exist.");
    }

    [Fact]
    public async void Handle_WhenUnauthorizedUser_ThrowForbiddenException()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand() { Id = restaurantId };
        Restaurant restaurantToUpdate = new Restaurant() { Id = restaurantId };

        _restaurantsRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurantToUpdate);

        _restaurantAuthorizationService
            .Setup(auth => auth.Authorize(restaurantToUpdate, ResourceOperations.Update))
            .Returns(false);

        // act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // asserts
        await action
            .Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage($"User not authorized to update restaurant with {restaurantId} id");
    }
}
