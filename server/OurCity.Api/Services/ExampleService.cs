using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OurCity.Api.Common;
using OurCity.Api.Services.Authorization;

namespace OurCity.Api.Services;

public interface IExampleService
{
    Task<Result<IEnumerable<WeatherForecastDto>>> GetWeatherForecasts(
        ClaimsPrincipal requestingUser
    );
}

public class ExampleService : IExampleService
{
    private readonly IAuthorizationService _authorizationService;

    public ExampleService(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<Result<IEnumerable<WeatherForecastDto>>> GetWeatherForecasts(
        ClaimsPrincipal requestingUser
    )
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            requestingUser,
            Policy.CanCreatePosts
        );

        if (!authorizationResult.Succeeded)
            return Result<IEnumerable<WeatherForecastDto>>.Unauthorized();

        return Result<IEnumerable<WeatherForecastDto>>.Success(
            Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecastDto
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = "Test",
                })
                .ToList()
        );
    }
}

public class WeatherForecastDto
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
