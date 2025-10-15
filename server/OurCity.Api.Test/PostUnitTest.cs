using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using OurCity.Api.Services;
using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Common;
using OurCity.Api.Common.Dtos;
using OurCity.Api.Common.Enum;

public class PostUnitTest
{
  private readonly Mock<IPostRepository> _mockRepo;
  private readonly PostService _service;

  public PostUnitTest()
  {
    _mockRepo = new Mock<IPostRepository>();
    _service = new PostService(_mockRepo.Object);
  }

  [Fact]
  public async Task GetPostsWithExistingPosts()
  {
    // Arrange
    var posts = new List<Post> {
      new Post { Id = 1, Title = "Test Post 1", Description = "Test Description 1" },
      new Post { Id = 2, Title = "Test Post 2", Description = "Test Description 2" },
    };
    _mockRepo.Setup(r => r.GetAllPosts()).ReturnsAsync(posts as IEnumerable<Post>);

    // Act
    var result = await _service.GetPosts();

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.Equal(2, result.Data.Count());
    Assert.Contains(result.Data, p => p.Id == 1);
  }

  [Fact]
  public async Task GetPostsWithNoPosts()
  {
    // Arrange
    _mockRepo.Setup(r => r.GetAllPosts()).ReturnsAsync(new List<Post>() as IEnumerable<Post>);

    // Act
    var result = await _service.GetPosts();

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.Empty(result.Data);
  }

  [Fact]
  public async Task GetPostByIdWithExistingPost()
  {
    // Arrange
    var post = new Post { Id = 1, Title = "Test Post", Description = "Test Description" };
    _mockRepo.Setup(r => r.GetPostById(1)).ReturnsAsync(post);

    // Act
    var result = await _service.GetPostById(1);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.Equal(1, result.Data.Id);
    Assert.Equal("Test Post", result.Data.Title);
  }

  [Fact]
  public async Task GetPostByIdWithNoExistingPost()
  {
    // Arrange
    _mockRepo.Setup(r => r.GetPostById(99)).ReturnsAsync((Post)null);

    // Act
    var result = await _service.GetPostById(99);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Null(result.Data);
    Assert.Equal("Post does not exist", result.Error);
  }

  [Fact]
  public async Task UserDidUpvote()
  {
    // Arrange
    var post = new Post { Id = 1, AuthorId = 10, Title = "Test Post", Description = "Test", UpvotedUserIds = new List<int> { 5 } };
    _mockRepo.Setup(r => r.GetPostById(1)).ReturnsAsync(post);

    // Act
    var result = await _service.GetUserUpvoteStatus(1, 5);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.Equal(1, result.Data.Id);
    Assert.Equal(10, result.Data.AuthorId);
    Assert.True(result.Data.Upvoted);
  }

  [Fact]
  public async Task UserDidNotUpvote()
  {
    // Arrange
    var post = new Post { Id = 1, AuthorId = 10, Title = "Test Post", Description = "Test", UpvotedUserIds = new List<int> { 7 } };
    _mockRepo.Setup(r => r.GetPostById(1)).ReturnsAsync(post);

    // Act
    var result = await _service.GetUserUpvoteStatus(1, 5);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.Equal(1, result.Data.Id);
    Assert.Equal(10, result.Data.AuthorId);
    Assert.False(result.Data.Upvoted);
  }

  [Fact]
  public async Task UserUpvoteNotExistingPost()
  {
    // Arrange
    _mockRepo.Setup(r => r.GetPostById(99)).ReturnsAsync((Post)null);

    // Act
    var result = await _service.GetUserUpvoteStatus(99, 5);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Null(result.Data);
    Assert.Equal("Post does not exist", result.Error);
  }

  [Fact]
  public async Task UserDidDownvote()
  {
    // Arrange
    var post = new Post { Id = 1, AuthorId = 10, Title = "Test Post", Description = "Test", DownvotedUserIds = new List<int> { 5 } };
    _mockRepo.Setup(r => r.GetPostById(1)).ReturnsAsync(post);

    // Act
    var result = await _service.GetUserDownvoteStatus(1, 5);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.Equal(1, result.Data.Id);
    Assert.Equal(10, result.Data.AuthorId);
    Assert.True(result.Data.Downvoted);
  }

  [Fact]
  public async Task UserDidNotDownvote()
  {
    // Arrange
    var post = new Post { Id = 1, AuthorId = 10, Title = "Test Post", Description = "Test", DownvotedUserIds = new List<int> { 7 } };
    _mockRepo.Setup(r => r.GetPostById(1)).ReturnsAsync(post);

    // Act
    var result = await _service.GetUserDownvoteStatus(1, 5);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Data);
    Assert.Equal(1, result.Data.Id);
    Assert.Equal(10, result.Data.AuthorId);
    Assert.False(result.Data.Downvoted);
  }

  [Fact]
  public async Task UserDownvoteNotExistingPost()
  {
    // Arrange
    _mockRepo.Setup(r => r.GetPostById(99)).ReturnsAsync((Post)null);

    // Act
    var result = await _service.GetUserDownvoteStatus(99, 5);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Null(result.Data);
    Assert.Equal("Post does not exist", result.Error);
  }

  // CreatePost
  // UpdatePost
  // VotePost
  // DeletePost
}