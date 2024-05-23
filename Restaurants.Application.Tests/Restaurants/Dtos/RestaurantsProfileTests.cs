using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantsProfileTests
{
    private IMapper _mapper;

    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // arrange
        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            Category = "Asian",
            HasDelivery = true,
            ContactEmail = "ContactEmail@test.com",
            ContactNumber = "12-123-123-123",
            Address = new Address()
            {
                City = "SinCity",
                Street = "Devlis avenue",
                PostalCode = "66-666"
            }
        };

        // act
        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // asserts

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var restaurantCommand = new CreateRestaurantCommand()
        {
            Name = "Name",
            Description = "Description",
            Category = "Asian",
            HasDelivery = true,
            ContactEmail = "ContactEmail@test.com",
            ContactNumber = "12-123-123-123",
            City = "SinCity",
            Street = "Devlis avenue",
            PostalCode = "66-666"
        };

        // act
        var restaurant = _mapper.Map<Restaurant>(restaurantCommand);

        // asserts

        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(restaurantCommand.Name);
        restaurant.Description.Should().Be(restaurantCommand.Description);
        restaurant.Category.Should().Be(restaurantCommand.Category);
        restaurant.HasDelivery.Should().Be(restaurantCommand.HasDelivery);
        restaurant.Address.City.Should().Be(restaurantCommand.City);
        restaurant.Address.Street.Should().Be(restaurantCommand.Street);
        restaurant.Address.PostalCode.Should().Be(restaurantCommand.PostalCode);
    }

    [Fact]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var restaurantCommand = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            HasDelivery = false
        };

        // act
        var restaurant = _mapper.Map<Restaurant>(restaurantCommand);

        // asserts

        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(restaurantCommand.Id);
        restaurant.Name.Should().Be(restaurantCommand.Name);
        restaurant.Description.Should().Be(restaurantCommand.Description);
        restaurant.HasDelivery.Should().Be(restaurantCommand.HasDelivery);
    }
}
