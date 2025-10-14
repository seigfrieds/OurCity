using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace OurCity.Api.Authentication.Auth0;

/// <summary>
/// </summary>

/// <credits>
/// Took a lot from https://auth0.com/blog/backend-for-frontend-pattern-with-auth0-and-dotnet/#What-Is-the-Backend-For-Frontend-Authentication-Pattern-
/// </credits>

public static class AddAuth0ToServices
{
    public static IServiceCollection AddAuth0Authentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
            })
            .AddOpenIdConnect(
                "Auth0",
                options =>
                {
                    options.ClaimsIssuer = "Auth0";
                    options.Authority = $"https://{configuration["Auth0:Domain"]}";
                    options.ClientId = configuration["Auth0:ClientId"];
                    options.ClientSecret = configuration["Auth0:ClientSecret"];

                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    options.ResponseMode = OpenIdConnectResponseMode.FormPost;

                    options.Scope.Clear();
                    options.Scope.Add("openid");

                    options.CallbackPath = new PathString("/callback");

                    options.SaveTokens = true;

                    options.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProvider = context =>
                        {
                            context.ProtocolMessage.SetParameter(
                                "audience",
                                configuration["Auth0:ApiAudience"]
                            );

                            return Task.CompletedTask;
                        },

                        OnRedirectToIdentityProviderForSignOut = context =>
                        {
                            var logoutUri =
                                $"https://{configuration["Auth0:Domain"]}/v2/logout?client_id={configuration["Auth0:ClientId"]}";
                            /*var afterLogoutUri = context.Properties.RedirectUri;
                            
                            if (!string.IsNullOrEmpty(afterLogoutUri))
                            {
                                if (afterLogoutUri.StartsWith("/"))
                                {
                                    var request = context.Request;
                                    afterLogoutUri =
                                        request.Scheme
                                        + "://"
                                        + request.Host
                                        + request.PathBase
                                        + afterLogoutUri;
                                }
        
                                logoutUri += $"&returnTo={Uri.EscapeDataString(afterLogoutUri)}";
                            }*/

                            context.Response.Redirect(logoutUri);

                            return Task.CompletedTask;
                        },
                    };
                }
            );

        return services;
    }
}
