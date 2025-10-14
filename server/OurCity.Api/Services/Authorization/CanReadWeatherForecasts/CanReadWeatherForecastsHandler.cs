using Microsoft.AspNetCore.Authorization;

namespace OurCity.Api.Services.Authorization.CanReadWeatherForecasts;

public class CanReadWeatherForecastsHandler
    : AuthorizationHandler<CanReadWeatherForecastsRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanReadWeatherForecastsRequirement requirement
    )
    {
        var user = context.User;

        if (user.GetUsername() == "ResourceOwner")
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
