using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OurCity.Api.Test.EndpointTests;

[Trait("Type", "Endpoints")]
[Trait("Domain", "Posts")]
public class PostsEndpointsTests : IClassFixture<OurCityWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PostsEndpointsTests(OurCityWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMeWithoutLogin_ReturnsUnAuthorized()
    {
        var response = await _client.GetAsync("/Authentication/Me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task GetMeWithLogin_ReturnsOk()
    {
        await _client.PostAsync("/Authentication/Login/username", null);
        
        var response = await _client.GetAsync("/Authentication/Me");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetMeWithLoginThenLogout_ReturnsUnauthorized()
    {
        await _client.PostAsync("/Authentication/Login/username", null);
        var loginResponse = await _client.GetAsync("/Authentication/Me");
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        
        await _client.PostAsync("/Authentication/Logout", null);
        var logoutResponse = await _client.GetAsync("/Authentication/Me");
        Assert.Equal(HttpStatusCode.Unauthorized, logoutResponse.StatusCode);
    }
}
