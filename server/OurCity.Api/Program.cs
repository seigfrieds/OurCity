using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OurCity.Api.Authentication.Auth0;
using OurCity.Api.Authentication.Development;
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
        policy
            .WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? []
            )
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
if (builder.Configuration["AuthOption"] == "Auth0")
{
    builder.Services.AddAuth0Authentication(builder.Configuration);
}
else
{
    builder.Services.AddDevelopmentAuthentication();
}


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
