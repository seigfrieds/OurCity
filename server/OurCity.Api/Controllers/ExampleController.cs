using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OurCity.Api.Configurations;
using OurCity.Api.Services;
using OurCity.Api.Services.Dtos;

namespace OurCity.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
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
        "Scorching",
    };

    private readonly ILogger<ExampleController> _logger;
    private readonly IOptions<ExampleSettings> _exampleSettings;
    private readonly IPostService _postService;

    public ExampleController(
        IPostService postService,
        IOptions<ExampleSettings> exampleSettings,
        ILogger<ExampleController> logger
    )
    {
        _postService = postService;
        _exampleSettings = exampleSettings;
        _logger = logger;
    }

    //EXAMPLE -> TALKING TO SERVICE LAYER
    [HttpGet]
    [Route("Posts")]
    [EndpointSummary("This is a summary for GetPosts")]
    [EndpointDescription("This is a description for GetPosts")]
    [ProducesResponseType(typeof(List<PostResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postService.GetPosts();

        return Ok(posts);
    }

    //EXAMPLE -> USING DI SETTINGS
    [HttpGet]
    [Route("Settings")]
    [EndpointSummary("This is a summary for GetSettings")]
    [EndpointDescription("This is a description for GetSettings")]
    [ProducesResponseType(typeof(ExampleSettings), StatusCodes.Status200OK)]
    public IActionResult GetSettings()
    {
        _logger.LogInformation("I am processing a Settings request right now!");

        return Ok(_exampleSettings.Value);
    }

    //EXAMPLE -> PROBLEM RESULTS, LINQ STUFF (LIST OPERATIONS), ANNOTATIONS FOR PARAMS
    [HttpGet]
    [Route("WeatherForecast")]
    [EndpointSummary("This is a summary for GetWeatherForecast")]
    [EndpointDescription("This is a description for GetWeatherForecast")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<WeatherForecastDto>), StatusCodes.Status200OK)]
    public IActionResult GetWeatherForecast([Required] [FromQuery(Name = "fail")] bool fail)
    {
        _logger.LogInformation("I am processing a WeatherForecast request right now!");

        if (fail)
        {
            _logger.LogError("Weather forecast does not exist brah");

            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                detail: "Weather forecast does not exist"
            );
        }

        return Ok(
            Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecastDto
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                })
                .ToList()
        );
    }
}

class WeatherForecastDto
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
