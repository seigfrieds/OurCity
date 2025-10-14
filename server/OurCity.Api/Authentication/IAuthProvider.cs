using Microsoft.AspNetCore.Mvc;

namespace OurCity.Api.Authentication;

/// <summary>
/// </summary>

/// <credits>
/// Code taken largely from ChatGPT prompt asking for AuthenticationController to take DI'd auth provider so I can switch between Dev and Auth0 authentication
/// </credits>

public interface IAuthProvider
{
    Task<AuthResult> AuthenticateAsync(HttpContext httpContext, string? returnUrl = null);
    Task<AuthResult> LogoutAsync(HttpContext httpContext);
}