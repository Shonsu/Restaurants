using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IRestaurantsService _restaurantsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await _restaurantsService.GetAllRestaurants();
        return Ok(restaurants);
    }

    [HttpGet("{restaurantId}")]
    public async Task<IActionResult> GetById(int restaurantId)
    {
        var restaurant = await _restaurantsService.GetRestaurantById(restaurantId);
        if (restaurant == null)
        {
            return NotFound($"Restaurant with id: {restaurantId} doesn't exist.");
        }
        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(
        [FromBody] CreateRestaurantDto createRestaurantDto
    )
    {
        // if(!ModelState.IsValid){
        //     return BadRequest(ModelState);
        // }
        int id = await _restaurantsService.CreateRestaurant(createRestaurantDto);
        return CreatedAtAction(nameof(GetById), new { restaurantId = id }, null);
    }
}
