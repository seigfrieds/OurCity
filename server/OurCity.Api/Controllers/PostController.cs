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

    public PostController(
        IPostService postService,
        ILogger<PostController> logger
    )
    {
        _postService = postService;
        _logger = logger;
    }

    [HttpGet]
    [Route("Posts")]
    [EndpointSummary("This is a summary for GetPosts")]
    [EndpointDescription("This is a description for GetPosts")]
    [ProducesResponseType(typeof(List<PostDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postService.GetPosts();

        return Ok(posts);
    }

    [HttpPost]
    [Route("Posts")]
    [EndpointSummary("Create a new post")]
    [EndpointDescription("Creates a new post with the provided data")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePost(PostDto postDto)
    {
        var post = await _postService.CreatePost(postDto);

        if (!post.IsSuccess)
        {
            return BadRequest(post.Error);
        }

        return CreatedAtAction(nameof(GetPosts), new { id = post.Data?.Id }, post.Data);
    }
}
