using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> restaurantRepositoryMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(service =>
            {
                service.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                service.Replace(
                    ServiceDescriptor.Scoped(
                        typeof(IRestaurantsRepository),
                        _ => restaurantRepositoryMock.Object
                    )
                );
            })
        );
    }

    [Fact]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // arrange
        var id = 123123;
        var client = _factory.CreateClient();
        restaurantRepositoryMock.Setup(rr => rr.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        // act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        // arrange
        var id = 99;
        var client = _factory.CreateClient();
        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "NameTest",
            Description = "Description test"
        };
        restaurantRepositoryMock
            .Setup(rr => rr.GetByIdAsync(id))
            .ReturnsAsync(restaurant);

        // act
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();
        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
    }

    [Fact]
    public async void GetAll_ForValidRequest_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=5");

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var result = await client.GetAsync("/api/restaurants");

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
