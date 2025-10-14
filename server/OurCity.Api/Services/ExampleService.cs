using System.Security.Claims;
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
    private readonly IPolicyService _policyService;

    public ExampleService(IPolicyService policyService)
    {
        _policyService = policyService;
    }

    public async Task<Result<IEnumerable<WeatherForecastDto>>> GetWeatherForecasts(
        ClaimsPrincipal requestingUser
    )
    {
        var isAuthorized = await _policyService.CheckPolicy(
            requestingUser,
            Policy.CanReadWeatherForecasts
        );

        if (!isAuthorized)
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
