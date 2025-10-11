using OurCity.Api.Infrastructure;
using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Services.Dtos;
using OurCity.Api.Services.Mappings;
using OurCity.Api.Common; 

namespace OurCity.Api.Services;

public interface IPostService
{
    Task<IEnumerable<PostResponseDto>> GetPosts();
    Task<Result<PostResponseDto>> CreatePost(PostRequestDto postDto);
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

    public async Task<Result<PostResponseDto>> CreatePost(PostRequestDto postDto)
    {
        if (postDto == null)
        {
            return Result<PostResponseDto>.Failure("Invalid Post Data.");
        }

        var post = new Post
        {
            Title = postDto.Title,
            Description = postDto.Description
        };
        var createdPost = await _postRepository.CreatePost(post);
        return Result<PostResponseDto>.Success(createdPost.ToDto());
    }
}
