namespace Restaurants.API;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> Get(int numberOfResult, int minTemp, int maxTemp);
}

public class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };

    public IEnumerable<WeatherForecast> Get(int numberOfResult, int minTemp, int maxTemp)
    {
        return Enumerable
            .Range(1, numberOfResult)
            .Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(minTemp, maxTemp),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}
