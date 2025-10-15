using Microsoft.AspNetCore.Mvc;
using OurCity.Api.Common.Dtos.Image;
using OurCity.Api.Services;

namespace OurCity.Api.Controllers;

[ApiController]
[Route("[controller]s")]
public class ImageController : ControllerBase
{
    private readonly ILogger<ImageController> _logger;
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService, ILogger<ImageController> logger)
    {
        _imageService = imageService;
        _logger = logger;
    }

    [HttpPost]
    [Route("upload/{postId}")]
    [EndpointSummary("Upload images for a post")]
    [EndpointDescription("Uploads one or more images to an existing post")]
    [ProducesResponseType(typeof(List<ImageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadImages(int postId, [FromForm] List<IFormFile> files)
    {
        var images = await _imageService.UploadImages(postId, files);

        if (!images.IsSuccess)
        {
            return NotFound(images.Error);
        }
        
        return Ok(images.Data);

    }
}