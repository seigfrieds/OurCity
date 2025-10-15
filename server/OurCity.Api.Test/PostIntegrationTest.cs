using Microsoft.EntityFrameworkCore;
using OurCity.Api.Common.Dtos;
using OurCity.Api.Common.Enum;
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
    private User _testUser = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _dbContext = new AppDbContext(options);

        await _dbContext.Database.EnsureCreatedAsync();

        // Seed a generic user for tests
        _testUser = new User
        {
            Id = 1,
            DisplayName = "Dummy",
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
            AuthorId = _testUser.Id,
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
            Assert.Equal(createdPost.Data.Location, retrievedPost.Data.Location);
            Assert.Equal(createdPost.Data.UpvotedUserIds, retrievedPost.Data.UpvotedUserIds);
            Assert.Equal(createdPost.Data.DownvotedUserIds, retrievedPost.Data.DownvotedUserIds);
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
            AuthorId = _testUser.Id,
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
            Assert.Null(createdPost.Data.Location);
            Assert.Empty(createdPost.Data.UpvotedUserIds);
            Assert.Empty(createdPost.Data.DownvotedUserIds);
            Assert.Empty(createdPost.Data.Images);
        });
    }

    [Fact]
    public async Task UpdatePostShouldModifyAndReturnPost()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            AuthorId = _testUser.Id,
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
            Assert.Equal("New Location", updatedPost.Data.Location);
            Assert.Empty(createdPost.Data.UpvotedUserIds);
            Assert.Empty(createdPost.Data.DownvotedUserIds);
            Assert.Empty(updatedPost.Data.Images);
        });
    }

    [Fact]
    public async Task UpvoteShouldAddUserIdToUpvotedList()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            AuthorId = _testUser.Id,
            Title = "Test Post",
            Description = "This is a test post",
        };

        var createdPost = await postService.CreatePost(createDto);
        Assert.NotNull(createdPost.Data);
        Assert.Empty(createdPost.Data.UpvotedUserIds);
        Assert.Empty(createdPost.Data.DownvotedUserIds);

        var userId = 1;
        var upvotedPost = await postService.VotePost(createdPost.Data.Id, userId, VoteType.Upvote);

        Assert.True(upvotedPost.IsSuccess);
        Assert.NotNull(upvotedPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Single(upvotedPost.Data.UpvotedUserIds);
            Assert.Contains(userId, upvotedPost.Data.UpvotedUserIds);
            Assert.Empty(upvotedPost.Data.DownvotedUserIds);
            Assert.DoesNotContain(userId, upvotedPost.Data.DownvotedUserIds);
        });

        var removedUpvotePost = await postService.VotePost(
            createdPost.Data.Id,
            userId,
            VoteType.Upvote
        );

        Assert.True(removedUpvotePost.IsSuccess);
        Assert.NotNull(removedUpvotePost.Data);

        Assert.Multiple(() =>
        {
            Assert.Empty(removedUpvotePost.Data.UpvotedUserIds);
            Assert.DoesNotContain(userId, removedUpvotePost.Data.UpvotedUserIds);
            Assert.Empty(removedUpvotePost.Data.DownvotedUserIds);
            Assert.DoesNotContain(userId, removedUpvotePost.Data.DownvotedUserIds);
        });
    }

    [Fact]
    public async Task DownvoteShouldAddUserIdToDownvotedList()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            AuthorId = _testUser.Id,
            Title = "Test Post",
            Description = "This is a test post",
        };

        var createdPost = await postService.CreatePost(createDto);
        Assert.NotNull(createdPost.Data);
        Assert.Empty(createdPost.Data.UpvotedUserIds);
        Assert.Empty(createdPost.Data.DownvotedUserIds);

        var userId = 1;
        var downvotedPost = await postService.VotePost(
            createdPost.Data.Id,
            userId,
            VoteType.Downvote
        );

        Assert.True(downvotedPost.IsSuccess);
        Assert.NotNull(downvotedPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Empty(downvotedPost.Data.UpvotedUserIds);
            Assert.DoesNotContain(userId, downvotedPost.Data.UpvotedUserIds);
            Assert.Single(downvotedPost.Data.DownvotedUserIds);
            Assert.Contains(userId, downvotedPost.Data.DownvotedUserIds);
        });

        var removedDownvotePost = await postService.VotePost(
            createdPost.Data.Id,
            userId,
            VoteType.Downvote
        );

        Assert.True(removedDownvotePost.IsSuccess);
        Assert.NotNull(removedDownvotePost.Data);

        Assert.Multiple(() =>
        {
            Assert.Empty(removedDownvotePost.Data.UpvotedUserIds);
            Assert.DoesNotContain(userId, removedDownvotePost.Data.UpvotedUserIds);
            Assert.Empty(removedDownvotePost.Data.DownvotedUserIds);
            Assert.DoesNotContain(userId, removedDownvotePost.Data.DownvotedUserIds);
        });
    }

    [Fact]
    public async Task UpvoteShouldRemoveUserFromDownvoteListIfExists()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            AuthorId = _testUser.Id,
            Title = "Test Post",
            Description = "This is a test post",
        };

        var createdPost = await postService.CreatePost(createDto);
        Assert.NotNull(createdPost.Data);
        Assert.Empty(createdPost.Data.UpvotedUserIds);
        Assert.Empty(createdPost.Data.DownvotedUserIds);

        var userId = 1;
        var downvotedPost = await postService.VotePost(
            createdPost.Data.Id,
            userId,
            VoteType.Downvote
        );

        Assert.True(downvotedPost.IsSuccess);
        Assert.NotNull(downvotedPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Empty(downvotedPost.Data.UpvotedUserIds);
            Assert.DoesNotContain(userId, downvotedPost.Data.UpvotedUserIds);
            Assert.Single(downvotedPost.Data.DownvotedUserIds);
            Assert.Contains(userId, downvotedPost.Data.DownvotedUserIds);
        });

        var upvotedPost = await postService.VotePost(createdPost.Data.Id, userId, VoteType.Upvote);

        Assert.True(upvotedPost.IsSuccess);
        Assert.NotNull(upvotedPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Single(upvotedPost.Data.UpvotedUserIds);
            Assert.Contains(userId, upvotedPost.Data.UpvotedUserIds);
            Assert.Empty(upvotedPost.Data.DownvotedUserIds);
            Assert.DoesNotContain(userId, upvotedPost.Data.DownvotedUserIds);
        });
    }

    [Fact]
    public async Task DownvoteShouldRemoveUserFromUpvoteListIfExists()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            AuthorId = _testUser.Id,
            Title = "Test Post",
            Description = "This is a test post",
        };

        var createdPost = await postService.CreatePost(createDto);
        Assert.NotNull(createdPost.Data);
        Assert.Empty(createdPost.Data.UpvotedUserIds);
        Assert.Empty(createdPost.Data.DownvotedUserIds);

        var userId = 1;
        var upvotedPost = await postService.VotePost(createdPost.Data.Id, userId, VoteType.Upvote);

        Assert.True(upvotedPost.IsSuccess);
        Assert.NotNull(upvotedPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Single(upvotedPost.Data.UpvotedUserIds);
            Assert.Contains(userId, upvotedPost.Data.UpvotedUserIds);
            Assert.Empty(upvotedPost.Data.DownvotedUserIds);
            Assert.DoesNotContain(userId, upvotedPost.Data.DownvotedUserIds);
        });

        var downvotedPost = await postService.VotePost(
            createdPost.Data.Id,
            userId,
            VoteType.Downvote
        );

        Assert.True(downvotedPost.IsSuccess);
        Assert.NotNull(downvotedPost.Data);

        Assert.Multiple(() =>
        {
            Assert.Empty(downvotedPost.Data.UpvotedUserIds);
            Assert.DoesNotContain(userId, downvotedPost.Data.UpvotedUserIds);
            Assert.Single(downvotedPost.Data.DownvotedUserIds);
            Assert.Contains(userId, downvotedPost.Data.DownvotedUserIds);
        });
    }

    [Fact]
    public async Task DeletePostShouldRemoveAndReturnPost()
    {
        var postService = new PostService(new PostRepository(_dbContext));
        var createDto = new PostCreateRequestDto
        {
            AuthorId = _testUser.Id,
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
