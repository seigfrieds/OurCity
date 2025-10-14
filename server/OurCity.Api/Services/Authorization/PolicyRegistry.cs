using Microsoft.AspNetCore.Authorization;
using OurCity.Api.Services.Authorization.CanReadWeatherForecasts;

namespace OurCity.Api.Services.Authorization;

public class Policy
{
    public static readonly Policy CanReadWeatherForecasts = new("CanReadWeatherForecasts");

    private string Value { get; }

    private Policy(string value) => Value = value;

    public static implicit operator string(Policy p) => p.Value;
}

public static class PolicyRegistry
{
    public static AuthorizationOptions AddOurCityPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(
            Policy.CanReadWeatherForecasts,
            policy => policy.Requirements.Add(new CanReadWeatherForecastsRequirement())
        );

        return options;
    }
}
