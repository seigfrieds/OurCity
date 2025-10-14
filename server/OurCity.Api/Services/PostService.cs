using OurCity.Api.Common;
using OurCity.Api.Infrastructure;
using OurCity.Api.Services.Dtos;
using OurCity.Api.Services.Mappings;

namespace OurCity.Api.Services;

public interface IPostService
{
    Task<IEnumerable<PostResponseDto>> GetPosts();
    Task<Result<PostResponseDto>> CreatePost(PostRequestDto postRequestDto);
}

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PostResponseDto>> GetPosts()
    {
        return (await _postRepository.GetAllPosts()).ToDtos();
    }

    public async Task<Result<PostResponseDto>> CreatePost(PostRequestDto postRequestDto)
    {
        var createdPost = await _postRepository.CreatePost(postRequestDto.ToEntity());
        return Result<PostResponseDto>.Success(createdPost.ToDto());
    }
}
