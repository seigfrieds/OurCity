using Microsoft.EntityFrameworkCore;
using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Services;
using OurCity.Api.Services.Dtos;
using Testcontainers.PostgreSql;

namespace OurCity.Api.Test;

public class PostIntegrationTest : IAsyncLifetime
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
        var postService = new PostService(new PostRepository(_dbContext));
        var posts = await postService.GetPosts();
        Assert.Empty(posts);
    }

    [Fact]
    public async Task CreatePostShouldAddAndReturnPost()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var post = await postService.CreatePost(
            new PostRequestDto { Title = "Test Post", Description = "This is a test post" }
        );

        Assert.Same(post.Data?.Title, "Test Post");
        Assert.Same(post.Data?.Description, "This is a test post");
        Assert.Equal(post.Data?.Votes, 0);
        Assert.Null(post.Data?.Location);
        Assert.Empty(post.Data?.Images ?? []);
    }
}
