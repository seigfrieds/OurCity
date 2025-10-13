using Microsoft.EntityFrameworkCore;
using OurCity.Api.Infrastructure.Database;

namespace OurCity.Api.Infrastructure;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPosts();
    Task<Post> GetPostById(int postId);
    Task<Post> CreatePost(Post post);
    Task<Post> UpdatePost(Post post);
    Task<Post> DeletePost(Post post); 
}

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _appDbContext;

    public PostRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<Post>> GetAllPosts()
    {
        return await _appDbContext.Posts.ToListAsync();
    }

    public async Task<Post> GetPostById(int postId)
    {
        return await _appDbContext.Posts.FindAsync(postId);
    }

    public async Task<Post> CreatePost(Post post)
    {
        _appDbContext.Posts.Add(post);
        await _appDbContext.SaveChangesAsync();
        return post;
    }

    public async Task<Post> UpdatePost(Post post)
    {
        _appDbContext.Update(post);
        await _appDbContext.SaveChangesAsync();
        return post;
    }

    public async Task<Post> DeletePost(Post post)
    {
        _appDbContext.Posts.Remove(post);
        await _appDbContext.SaveChangesAsync();
        return post; 
    }
}
