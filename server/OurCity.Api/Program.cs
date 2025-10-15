using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OurCity.Api.Configurations;
using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Middlewares;
using OurCity.Api.Services;
using OurCity.Api.Services.Authorization;
using OurCity.Api.Services.Authorization.CanCreatePosts;
using OurCity.Api.Services.Authorization.CanMutateThisPost;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//App configuration
builder.Services.Configure<ExampleSettings>(builder.Configuration.GetSection("ExampleSettings"));
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));

//AWS S3 Client
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<AwsSettings>>().Value;

    if (string.IsNullOrEmpty(settings.Region))
        throw new InvalidOperationException("AWS Region is not configured");

    var config = new AmazonS3Config
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(settings.Region),
    };

    if (!string.IsNullOrEmpty(settings.AccessKey) && !string.IsNullOrEmpty(settings.SecretKey))
    {
        var credentials = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
        return new AmazonS3Client(credentials, config);
    }

    return new AmazonS3Client(config);
});

//Logging
builder.Host.UseSerilog((ctx, config) => config.ReadFrom.Configuration(builder.Configuration));

//Database
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

//Repository
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

//Service
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IExampleService, ExampleService>();

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
//NOTE: Stubbed, implementation not fully there
builder
    .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "OurCityAuthToken";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

//Authorization
builder.Services.AddSingleton<IAuthorizationHandler, CanCreatePostsHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CanMutateThisPostHandler>();
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
