using Microsoft.EntityFrameworkCore;
using OurCity.Api.Infrastructure.Database;

namespace OurCity.Api.Infrastructure;

public interface IImageRepository
{
    Task<IEnumerable<Image>> UploadImages(List<Image> images);
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
}
