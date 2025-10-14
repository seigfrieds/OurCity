using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OurCity.Api.Configurations;
using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Middlewares;
using OurCity.Api.Services;
using OurCity.Api.Services.Authorization;
using OurCity.Api.Services.Authorization.CanReadWeatherForecasts;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//App configuration
builder.Services.Configure<ExampleSettings>(builder.Configuration.GetSection("ExampleSettings"));

//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
            policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
            .AllowAnyHeader()
            .AllowAnyMethod()
        .AllowCredentials();
    });
});

//Logging
builder.Host.UseSerilog((ctx, config) => config.ReadFrom.Configuration(builder.Configuration));

//Database
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

//Repository
builder.Services.AddScoped<IPostRepository, PostRepository>();

//Service
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IExampleService, ExampleService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();

//Controller
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Extensions.Remove("traceId");
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//Authentication
//https://auth0.com/blog/backend-for-frontend-pattern-with-auth0-and-dotnet/#What-Is-the-Backend-For-Frontend-Authentication-Pattern-
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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
            options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
            options.ClientId = builder.Configuration["Auth0:ClientId"];
            options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];

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
                        builder.Configuration["Auth0:ApiAudience"]
                    );

                    return Task.CompletedTask;
                },
                
                OnRedirectToIdentityProviderForSignOut = context =>
                {
                    var logoutUri =
                        $"https://{builder.Configuration["Auth0:Domain"]}/v2/logout?client_id={builder.Configuration["Auth0:ClientId"]}";
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
                }
            };
        }
    );

//Authorization
builder.Services.AddSingleton<IAuthorizationHandler, CanReadWeatherForecastsHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddOurCityPolicies();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseCorrelationId();
app.UseSecurityHeaders();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //Multiple API documentation tools
    app.MapScalarApiReference();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.Run();
