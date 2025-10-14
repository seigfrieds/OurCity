using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OurCity.Api.Controllers;

/// <summary>
/// Code taken from //https://auth0.com/blog/backend-for-frontend-pattern-with-auth0-and-dotnet/#What-Is-the-Backend-For-Frontend-Authentication-Pattern-
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("Login")]
    [EndpointSummary("Yoo")]
    [EndpointDescription("Dasfire")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Login()
    {
        return new ChallengeResult("Auth0", new AuthenticationProperties { RedirectUri = "/" });
    }

    [HttpGet]
    [Authorize]
    [Route("Logout")]
    [EndpointSummary("Yoo")]
    [EndpointDescription("Dasfire")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        
        return new SignOutResult("Auth0", new AuthenticationProperties { RedirectUri = "/" });
    }
}
