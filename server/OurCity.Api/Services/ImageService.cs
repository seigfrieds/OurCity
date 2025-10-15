/// Generative AI - ChatGPT was used to assist in the creation of this file.
/// Prompt: Help me create a boiler plate for a C# logic that uploads images to AWS S3 and saves the image URLs to a database.

using Microsoft.AspNetCore.Mvc;
using OurCity.Api.Common;
using OurCity.Api.Common.Dtos.Image;
using OurCity.Api.Infrastructure;
using Amazon.S3;
using Amazon.S3.Transfer;
using OurCity.Api.Infrastructure.Database;

namespace OurCity.Api.Services;

public interface IImageService
{
    Task<Result<IEnumerable<ImageDto>>> UploadImages(int postId, [FromForm] List<IFormFile> files);
}

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IPostRepository _postRepository;
    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;

    public ImageService(IImageRepository imageRepository, IPostRepository postRepository, IAmazonS3 s3, IConfiguration configuration)
    {
        _imageRepository = imageRepository;
        _postRepository = postRepository;
        _s3 = s3;
        _bucketName = configuration.GetValue<string>("S3Settings:BucketName") ?? throw new ArgumentNullException("S3 bucket name is not configured");
    }

    public async Task<Result<IEnumerable<ImageDto>>> UploadImages(int postId, [FromForm] List<IFormFile> files)
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

        const long maxFileSize = 5 * 1024 * 1024; // 5MB
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        foreach (var file in files)
        {
            if (file.Length == 0)
                return Result<IEnumerable<ImageDto>>.Failure("Empty file detected");
                
            if (file.Length > maxFileSize)
                return Result<IEnumerable<ImageDto>>.Failure($"File {file.FileName} exceeds 5MB limit");
            
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
                    ContentType = file.ContentType
                };

                await transferUtility.UploadAsync(uploadRequest);

                var url = $"https://{_bucketName}.s3.amazonaws.com/{s3key}";
                var image = new Image
                {
                    Url = url,
                    PostId = postId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                images.Add(image);
                uploadedImages.Add(new ImageDto
                {
                    Url = url
                });
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
}