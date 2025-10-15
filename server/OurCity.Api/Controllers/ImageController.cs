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
    public async Task<IActionResult> UploadImages([FromRoute] int postId, [FromForm] IFormFileCollection files)
    {
        var images = await _imageService.UploadImages(postId, files);

        if (!images.IsSuccess)
        {
            return NotFound(images.Error);
        }

        return Ok(images.Data);
    }

    [HttpGet]
    [EndpointSummary("Get all images")]
    [EndpointDescription("Retrieves all images")]
    [ProducesResponseType(typeof(List<ImageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllImages()
    {
        var images = await _imageService.GetAllImages();

        return Ok(images.Data);
    }

    [HttpGet]
    [Route("{imageId}")]
    [EndpointSummary("Get an image by ID")]
    [EndpointDescription("Retrieves an image by its ID")]
    [ProducesResponseType(typeof(ImageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImageByImageId(int imageId)
    {
        var image = await _imageService.GetImageByImageId(imageId);

        if (!image.IsSuccess)
        {
            return NotFound(image.Error);
        }

        return Ok(image.Data);
    }

    [HttpGet]
    [Route("post/{postId}")]
    [EndpointSummary("Get an image by Post ID")]
    [EndpointDescription("Retrieves an image by its associated Post ID")]
    [ProducesResponseType(typeof(ImageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImageByPostId(int postId)
    {
        var image = await _imageService.GetImageByPostId(postId);
        
        if (!image.IsSuccess)
        {
            return NotFound(image.Error);
        }

        return Ok(image.Data);
    }
}
