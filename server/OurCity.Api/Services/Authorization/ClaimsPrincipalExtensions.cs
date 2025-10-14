using System.Security.Claims;

namespace OurCity.Api.Services.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.Identity?.Name;
    }
}
