using Microsoft.EntityFrameworkCore;
using OurCity.Api.Common.Dtos;
using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Services;
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
        var retrievedPost = await postService.GetPosts();

        Assert.True(retrievedPost.IsSuccess);
        Assert.NotNull(retrievedPost.Data);
        Assert.Empty(retrievedPost.Data);
    }

    [Fact]
    public async Task GettingExistingPostByIdShouldReturnPost()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            Title = "Test Post",
            Description = "This is a test post",
        };

        var createdPost = await postService.CreatePost(createDto);
        Assert.NotNull(createdPost.Data);

        var retrievedPost = await postService.GetPostById(createdPost.Data.Id);
        Assert.True(retrievedPost.IsSuccess);
        Assert.NotNull(retrievedPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Equal(createdPost.Data.Id, retrievedPost.Data.Id);
            Assert.Equal(createdPost.Data.Title, retrievedPost.Data.Title);
            Assert.Equal(createdPost.Data.Description, retrievedPost.Data.Description);
            Assert.Equal(createdPost.Data.Votes, retrievedPost.Data.Votes);
            Assert.Equal(createdPost.Data.Location, retrievedPost.Data.Location);
            Assert.Equal(
                createdPost.Data.Images.Select(i => i.Url),
                retrievedPost.Data.Images.Select(i => i.Url)
            );
        });
    }

    [Fact]
    public async Task CreatePostShouldAddAndReturnPost()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            Title = "Test Post",
            Description = "This is a test post",
        };

        var createdPost = await postService.CreatePost(createDto);
        Assert.True(createdPost.IsSuccess);
        Assert.NotNull(createdPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Equal("Test Post", createdPost.Data.Title);
            Assert.Equal("This is a test post", createdPost.Data.Description);
            Assert.Equal(0, createdPost.Data.Votes);
            Assert.Null(createdPost.Data.Location);
            Assert.Empty(createdPost.Data.Images);
        });
    }

    [Fact]
    public async Task UpdatePostShouldModifyAndReturnPost()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            Title = "Test Post",
            Description = "This is a test post",
        };

        var createdPost = await postService.CreatePost(createDto);
        Assert.NotNull(createdPost.Data);

        var updateDto = new PostUpdateRequestDto
        {
            Title = "Updated Test Post",
            Description = "This is an updatedPost test post",
            Location = "New Location",
        };

        var updatedPost = await postService.UpdatePost(createdPost.Data.Id, updateDto);

        Assert.True(updatedPost.IsSuccess);
        Assert.NotNull(updatedPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Equal("Updated Test Post", updatedPost.Data.Title);
            Assert.Equal("This is an updatedPost test post", updatedPost.Data.Description);
            Assert.Equal(0, updatedPost.Data.Votes);
            Assert.Equal("New Location", updatedPost.Data.Location);
            Assert.Empty(updatedPost.Data.Images);
        });
    }

    [Fact]
    public async Task DeletePostShouldRemoveAndReturnPost()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            Title = "Test Post",
            Description = "This is a test post",
        };

        var createdPost = await postService.CreatePost(createDto);
        Assert.NotNull(createdPost.Data);

        var deletedPost = await postService.DeletePost(createdPost.Data.Id);

        Assert.True(deletedPost.IsSuccess);
        Assert.NotNull(deletedPost.Data);

        var retrievedPost = await postService.GetPostById(deletedPost.Data.Id);
        Assert.False(retrievedPost.IsSuccess);
        Assert.Same(retrievedPost.Error, "Post does not exist");
    }
}
