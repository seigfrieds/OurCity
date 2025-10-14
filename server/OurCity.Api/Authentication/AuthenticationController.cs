using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurCity.Api.Authentication;

namespace OurCity.Api.Controllers;

/// <summary>
/// </summary>

/// <credits>
/// Code taken largely from ChatGPT prompt asking for AuthenticationController to take DI'd auth provider so I can switch between Dev and Auth0 authentication
/// </credits>

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthProvider _authProvider;

    public AuthenticationController(ILogger<AuthenticationController> logger, IAuthProvider authProvider)
    {
        _logger = logger;
        _authProvider = authProvider;
    }

    [HttpGet]
    [Route("Login")]
    [EndpointSummary("Yoo")]
    [EndpointDescription("Dasfire")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login()
    {
        var result = await _authProvider.AuthenticateAsync(HttpContext);

        return result.ActionType switch
        {
            AuthActionType.None => Ok(new { user = HttpContext.User.Identity?.Name }),
            AuthActionType.SignIn => await SignInAndReturnResult(result),
            AuthActionType.Redirect => Challenge(new AuthenticationProperties { RedirectUri = result.RedirectUrl }),
            _ => BadRequest()
        };
    }
    
    private async Task<IActionResult> SignInAndReturnResult(AuthResult result)
    {
        if (result.Principal != null)
        {
            await HttpContext.SignInAsync(result.Principal);
            return Ok(new { user = result.Principal.Identity?.Name });
        }

        return BadRequest();
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
        var result = await _authProvider.LogoutAsync(HttpContext);

        return result.ActionType switch
        {
            AuthActionType.SignIn => await SignInAndReturnResult(result),
            AuthActionType.Redirect => Challenge(new AuthenticationProperties { RedirectUri = result.RedirectUrl }),
            _ => Ok()
        };
    }
}
