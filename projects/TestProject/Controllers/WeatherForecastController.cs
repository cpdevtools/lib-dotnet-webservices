using System.ComponentModel.DataAnnotations;
using CpDevTools.Webservices.Models.Errors;
using Microsoft.AspNetCore.Mvc;

namespace TestProject.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Consumes("application/json")]
    [ProducesResponseType(statusCode: 200, type: typeof(WeatherForecast))]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost(Name = "Throw")]
    [Consumes("application/json")]
    [ProducesResponseType(statusCode: 500, type: typeof(ExceptionErrorModel))]
  
    public IEnumerable<WeatherForecast> Throw()
    {
        throw new KeyNotFoundException("oops");
    }

    [HttpPut(Name = "Invalid")]
    [Consumes("application/json")]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationErrorModel))]
  
    public IEnumerable<WeatherForecast> Invalid([FromBody][Required] string asdf)
    {
        
        return Get();
    }
}
