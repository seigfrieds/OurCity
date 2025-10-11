using OurCity.Api.Infrastructure;
using OurCity.Api.Services.Dtos;
using OurCity.Api.Services.Mappings;

namespace OurCity.Api.Services;

public interface IPostService
{
    Task<IEnumerable<PostDto>> GetPosts();
}

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PostDto>> GetPosts()
    {
        return (await _postRepository.GetAllPosts()).ToDtos();
    }
}
