/// Generative AI - ChatGPT was used to assist in the creation of this file.
/// Prompt: Help me create a boiler plate for a C# logic that uploads images to AWS S3 and saves the image URLs to a database.
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using OurCity.Api.Common;
using OurCity.Api.Common.Dtos.Image;
using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;

namespace OurCity.Api.Services;

public interface IImageService
{
    Task<Result<IEnumerable<ImageDto>>> UploadImages(int postId, IFormFileCollection files);
    Task<Result<IEnumerable<ImageDto>>> GetAllImages();
    Task<Result<ImageDto>> GetImageByImageId(int imageId);
    Task<Result<ImageDto>> GetImageByPostId(int postId);
}

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IPostRepository _postRepository;
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;

    public ImageService(
        IImageRepository imageRepository,
        IPostRepository postRepository,
        IAmazonS3 s3,
        IConfiguration configuration
    )
    {
        _imageRepository = imageRepository;
        _postRepository = postRepository;
        _s3 = s3;
        _bucketName =
            configuration.GetValue<string>("AwsSettings:BucketName")
            ?? throw new ArgumentNullException("S3 bucket name is not configured");
    }

    public async Task<Result<IEnumerable<ImageDto>>> UploadImages(
        int postId,
        IFormFileCollection files
    )
    {
        var post = await _postRepository.GetPostById(postId);

        if (post == null)
        {
            return Result<IEnumerable<ImageDto>>.Failure("Post does not exist");
        }

        if (files == null || files.Count == 0)
        {
            return Result<IEnumerable<ImageDto>>.Success(new List<ImageDto>());
        }

        const long maxFileSize = 5 * 1024 * 1024;
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        foreach (var file in files)
        {
            if (file.Length == 0)
                return Result<IEnumerable<ImageDto>>.Failure("Empty file detected");

            if (file.Length > maxFileSize)
                return Result<IEnumerable<ImageDto>>.Failure(
                    $"File {file.FileName} exceeds 5MB limit"
                );

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return Result<IEnumerable<ImageDto>>.Failure($"Invalid file type: {extension}");
        }

        var uploadedImages = new List<ImageDto>();
        var transferUtility = new TransferUtility(_s3);
        var images = new List<Image>();

        try
        {
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var s3key = $"posts/{postId}/{Guid.NewGuid()}{extension}";
                using var stream = file.OpenReadStream();
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = s3key,
                    BucketName = _bucketName,
                    ContentType = file.ContentType,
                };

                await transferUtility.UploadAsync(uploadRequest);

                var url = $"https://{_bucketName}.s3.amazonaws.com/{s3key}";
                var image = new Image
                {
                    Url = url,
                    PostId = postId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                images.Add(image);
                uploadedImages.Add(new ImageDto { Url = url });
            }

            await _imageRepository.UploadImages(images);
            return Result<IEnumerable<ImageDto>>.Success(uploadedImages);
        }
        catch (AmazonS3Exception ex)
        {
            return Result<IEnumerable<ImageDto>>.Failure($"Error uploading images: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ImageDto>>.Failure($"Unknown error: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<ImageDto>>> GetAllImages()
    {
        var images = await _imageRepository.GetAllImages();
        var imageDtos = images.Select(image => new ImageDto { Url = image.Url });
        return Result<IEnumerable<ImageDto>>.Success(imageDtos);
    }

    public async Task<Result<ImageDto>> GetImageByImageId(int imageId)
    {
        var image = await _imageRepository.GetImageByImageId(imageId);

        if (image == null)
        {
            return Result<ImageDto>.Failure("Image not found");
        }

        var imageDto = new ImageDto { Url = image.Url };
        return Result<ImageDto>.Success(imageDto);
    }
    
    public async Task<Result<ImageDto>> GetImageByPostId(int postId)
    {
        var image = await _imageRepository.GetImageByPostId(postId);

        if (image == null)
        {
            return Result<ImageDto>.Failure("Image not found");
        }

        var imageDto = new ImageDto { Url = image.Url };
        return Result<ImageDto>.Success(imageDto);
    }
}
