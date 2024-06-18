using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constans;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
    // [Authorize(Policy = PolicyNames.OwnerAtLeast2restaurant)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll(
        [FromQuery] GetAllRestaurantsQuery query
    )
    {
        var restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    [HttpGet("{restaurantId}")]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
    //[Authorize(Policy = PolicyNames.HasNationality)]
    public async Task<ActionResult<RestaurantDto>> GetById(int restaurantId)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(restaurantId));
        return Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    public async Task<IActionResult> CreateRestaurant(
        [FromBody] CreateRestaurantCommand createRestaurantCommand
    )
    {
        // if(!User.IsInRole("Owner")){
        //     return Unauthorized();
        // }
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
        await mediator.Send(new DeleteRestaurantCommand(restaurantId));
        return NoContent();
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
        await mediator.Send(updateRestaurantCommand);
        return NoContent();
    }

    [HttpPost("{restaurantId}/logo")]
    public async Task<IActionResult> UploadLogo([FromRoute] int restaurantId, IFormFile file)
    {
        Stream stream = file.OpenReadStream();
        var command = new UploadRestaurantLogoCommand
        {
            RestaurantId = restaurantId,
            FileName = file.FileName,
            File = stream
        };

        await mediator.Send(command);
        return NoContent();
    }
}
