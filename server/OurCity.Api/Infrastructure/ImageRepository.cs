using Microsoft.EntityFrameworkCore;
using OurCity.Api.Infrastructure.Database;

namespace OurCity.Api.Infrastructure;

public interface IImageRepository
{
    Task<IEnumerable<Image>> UploadImages(List<Image> images);
    Task<IEnumerable<Image>> GetAllImages();
    Task<Image?> GetImageByImageId(int imageId);
    Task<Image?> GetImageByPostId(int postId);
}

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _appDbContext;

    public ImageRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<Image>> UploadImages(List<Image> images)
    {
        _appDbContext.Images.AddRange(images);
        await _appDbContext.SaveChangesAsync();
        return images;
    }

    public async Task<IEnumerable<Image>> GetAllImages()
    {
        return await _appDbContext.Images.ToListAsync();
    }

    public async Task<Image?> GetImageByImageId(int imageId)
    {
        return await _appDbContext.Images.FirstOrDefaultAsync(img => img.Id == imageId);
    }

    public async Task<Image?> GetImageByPostId(int postId)
    {
        return await _appDbContext.Images.FirstOrDefaultAsync(img => img.PostId == postId);
    }
}
