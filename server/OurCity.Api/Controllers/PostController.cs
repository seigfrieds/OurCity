using Microsoft.AspNetCore.Mvc;
using OurCity.Api.Services;
using OurCity.Api.Services.Dtos;

namespace OurCity.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostService _postService;

    public PostController(IPostService postService, ILogger<PostController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    [HttpGet]
    [Route("Posts")]
    [EndpointSummary("Get all posts")]
    [EndpointDescription("Gets a list of all posts")]
    [ProducesResponseType(typeof(List<PostResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postService.GetPosts();

        return Ok(posts);
    }

    [HttpGet]
    [Route("Posts/{postId}")]
    [EndpointSummary("Get a post by ID")]
    [EndpointDescription("Gets a post by its ID")]
    [ProducesResponseType(typeof(PostResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPostById(int postId)
    {
        var post = await _postService.GetPostById(postId);
        
        if (!post.IsSuccess)
        {
            return NotFound(post.Error);
        }

        return Ok(post.Data);
    }

    [HttpPost]
    [Route("Posts")]
    [EndpointSummary("Create a new post")]
    [EndpointDescription("Creates a new post with the provided data")]
    [ProducesResponseType(typeof(PostResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePost(PostRequestDto postRequestDto)
    {
        var post = await _postService.CreatePost(postRequestDto);

        return CreatedAtAction(nameof(GetPosts), new { id = post.Data?.Id }, post.Data);
    }
}
