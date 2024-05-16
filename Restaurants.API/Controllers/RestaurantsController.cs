using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{restaurantId}")]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
    public async Task<ActionResult<RestaurantDto>> GetById(int restaurantId)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(restaurantId));
        if (restaurant is null)
        {
            return NotFound($"Restaurant with id: {restaurantId} doesn't exist.");
        }
        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(
        [FromBody] CreateRestaurantCommand createRestaurantCommand
    )
    {
        // if(!ModelState.IsValid){
        //     return BadRequest(ModelState);
        // }
        int id = await mediator.Send(createRestaurantCommand);
        return CreatedAtAction(nameof(GetById), new { restaurantId = id }, null);
    }

    [HttpDelete("{restaurantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int restaurantId)
    {
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(restaurantId));
        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPatch("{restaurantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant(
        [FromRoute] int restaurantId,
        [FromBody] UpdateRestaurantCommand updateRestaurantCommand
    )
    {
        updateRestaurantCommand.Id = restaurantId;
        bool result = await mediator.Send(updateRestaurantCommand);
        if (result)
        {
            return NoContent();
        }

        return NotFound();
    }
}
