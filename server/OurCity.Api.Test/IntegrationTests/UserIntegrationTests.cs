using Microsoft.EntityFrameworkCore;
using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Services;
using Testcontainers.PostgreSql;

namespace OurCity.Api.Test.IntegrationTests;

public class UserIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:16.10")
        .Build();
    private AppDbContext _dbContext = null!; //null! -> tell compiler to trust it will be initialized

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _dbContext = new AppDbContext(options);

        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task FreshDbShouldReturnNothing()
    {
        var userService = new UserService(new UserRepository(_dbContext));
        var retrievedUsers = await userService.GetUsers();

        Assert.NotNull(retrievedUsers);
        Assert.Empty(retrievedUsers);
    }
}
