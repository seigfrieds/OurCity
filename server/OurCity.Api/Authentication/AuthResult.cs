using System.Security.Claims;

namespace OurCity.Api.Authentication;

/// <summary>
/// </summary>

/// <credits>
/// Code taken largely from ChatGPT prompt asking for AuthenticationController to take DI'd auth provider so I can switch between Dev and Auth0 authentication
/// </credits>

public enum AuthActionType
{
    None,           // Already authenticated
    SignIn,         // Create cookie/JWT
    Redirect        // Requires OIDC redirect / external login
}

public class AuthResult
{
    public AuthActionType ActionType { get; set; }
    public ClaimsPrincipal? Principal { get; set; } // for SignIn
    public string? RedirectUrl { get; set; }       // for Redirect
    public string? Token { get; set; }             // optional JWT token
}