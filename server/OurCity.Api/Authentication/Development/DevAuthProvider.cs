using System.Security.Claims;

namespace OurCity.Api.Authentication.Development;

/// <summary>
/// </summary>

/// <credits>
/// Code taken largely from ChatGPT prompt asking for AuthenticationController to take DI'd auth provider so I can switch between Dev and Auth0 authentication
/// </credits>

public class DevAuthProvider : IAuthProvider
{
    public Task<AuthResult> AuthenticateAsync(HttpContext httpContext, string? returnUrl = null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "dev.local"),
            new Claim(ClaimTypes.Name, "dev.local")
        };
        var identity = new ClaimsIdentity(claims, "DevAuth");
        var principal = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthResult
        {
            ActionType = AuthActionType.SignIn,
            Principal = principal
        });
    }

    public Task<AuthResult> LogoutAsync(HttpContext httpContext)
    {
        return Task.FromResult(new AuthResult { ActionType = AuthActionType.None });
    }
}