using Microsoft.EntityFrameworkCore;
using OurCity.Api.Common.Dtos.User;
using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Services;
using Testcontainers.PostgreSql;

namespace OurCity.Api.Test.IntegrationTests;

[Trait("Type", "Integration")]
[Trait("Domain", "User")]
public class UserIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:16.10")
        .Build();
    private AppDbContext _dbContext = null!; //null! -> tell compiler to trust it will be initialized
    private User _testUser = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _dbContext = new AppDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();

        _testUser = new User
        {
            Username = "Test",
            DisplayName = "Display Test",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        _dbContext.Users.Add(_testUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task GetCreatedUserShouldSucceed()
    {
        var userService = new UserService(new UserRepository(_dbContext));

        var retrievedUser = await userService.GetUserById(_testUser.Id);
        Assert.True(retrievedUser.IsSuccess);
        Assert.NotNull(retrievedUser.Data);

        Assert.Multiple(() =>
        {
            Assert.Equal(_testUser.Username, retrievedUser.Data.Username);
            Assert.Equal(_testUser.DisplayName, retrievedUser.Data.DisplayName);
        });
    }

    [Fact]
    public async Task CreateUserShouldAddAndReturnUser()
    {
        var userService = new UserService(new UserRepository(_dbContext));
        var createDto = new UserCreateRequestDto { Username = "Test Username" };

        var createdUser = await userService.CreateUser(createDto);
        Assert.True(createdUser.IsSuccess);
        Assert.NotNull(createdUser.Data);

        Assert.Multiple(() =>
        {
            Assert.Equal("Test Username", createdUser.Data.Username);
            Assert.Null(createdUser.Data.DisplayName);
            Assert.False(createdUser.Data.IsDeleted);
        });
    }
}
