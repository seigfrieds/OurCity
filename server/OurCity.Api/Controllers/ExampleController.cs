using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OurCity.Api.Common;
using OurCity.Api.Configurations;
using OurCity.Api.Services;
using OurCity.Api.Services.Dtos;

namespace OurCity.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    private readonly ILogger<ExampleController> _logger;
    private readonly IOptions<ExampleSettings> _exampleSettings;
    private readonly IPostService _postService;
    private readonly IExampleService _exampleService;

    public ExampleController(
        IPostService postService,
        IExampleService exampleService,
        IOptions<ExampleSettings> exampleSettings,
        ILogger<ExampleController> logger
    )
    {
        _postService = postService;
        _exampleService = exampleService;
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

    //EXAMPLE -> PROBLEM RESULTS, LINQ STUFF (LIST OPERATIONS), ANNOTATIONS FOR PARAMS, AUTH
    [HttpGet]
    [Authorize]
    [Route("WeatherForecast")]
    [EndpointSummary("This is a summary for GetWeatherForecast")]
    [EndpointDescription("This is a description for GetWeatherForecast")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<WeatherForecastDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWeatherForecast(
        [Required] [FromQuery(Name = "fail")] bool fail
    )
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

        var result = await _exampleService.GetWeatherForecasts(HttpContext.User);

        if (!result.IsSuccess)
            return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: result.Error);

        return Ok(result.Data);
    }
}
