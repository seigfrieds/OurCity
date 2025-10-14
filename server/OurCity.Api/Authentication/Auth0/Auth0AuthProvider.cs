using Microsoft.AspNetCore.Authentication;

namespace OurCity.Api.Authentication.Auth0;

/// <summary>
/// </summary>

/// <credits>
/// Code taken largely from ChatGPT prompt asking for AuthenticationController to take DI'd auth provider so I can switch between Dev and Auth0 authentication
/// </credits>

public class Auth0Provider : IAuthProvider
{
    public Task<AuthResult> AuthenticateAsync(HttpContext httpContext, string? returnUrl = null)
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = returnUrl ?? "/"
        };

        return Task.FromResult(new AuthResult
        {
            ActionType = AuthActionType.Redirect,
            RedirectUrl = props.RedirectUri
        });
    }

    public Task<AuthResult> LogoutAsync(HttpContext httpContext)
    {
        var props = new AuthenticationProperties { RedirectUri = "/" };
        return Task.FromResult(new AuthResult
        {
            ActionType = AuthActionType.Redirect,
            RedirectUrl = props.RedirectUri
        });
    }
}