using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IWeatherForecastService weatherForecastService
    )
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        //Response.StatusCode = 400;
        //return StatusCode(400,_weatherForecastService.Get());

        return BadRequest(_weatherForecastService.Get(5, -20, 35));
    }

    [HttpPost("generate")]
    public IActionResult GenerateWeather([FromQuery] int numberOfResult, [FromBody] TemperatureRequest range)
    {
        if (numberOfResult < 0 || range.Min > range.Max)
        {
            return BadRequest("Number of result has to be positive number and max value must be grater than min value");
        }
        return Ok(_weatherForecastService.Get(numberOfResult, range.Min, range.Max));
    }

    public record TemperatureRequest(int Min, int Max);
}
