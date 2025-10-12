using OurCity.Api.Infrastructure;
using OurCity.Api.Common;
using OurCity.Api.Common.Dtos;
using OurCity.Api.Services.Mappings;

namespace OurCity.Api.Services;

public interface IPostService
{
    Task<Result<IEnumerable<PostResponseDto>>> GetPosts();
    Task<Result<PostResponseDto>> GetPostById(int postId);
    Task<Result<PostResponseDto>> CreatePost(PostCreateRequestDto postRequestDto);
    Task<Result<PostResponseDto>> UpdatePost(int postId, PostUpdateRequestDto postRequestDto);
}

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Result<IEnumerable<PostResponseDto>>> GetPosts()
    {
        var posts = await _postRepository.GetAllPosts();
        return Result<IEnumerable<PostResponseDto>>.Success(posts.ToDtos());
    }

    public async Task<Result<PostResponseDto>> GetPostById(int postId)
    {
        var post = await _postRepository.GetPostById(postId);

        if (post == null)
        {
            return Result<PostResponseDto>.Failure("Post does not exist");
        }

        return Result<PostResponseDto>.Success(post.ToDto());
    }

    public async Task<Result<PostResponseDto>> CreatePost(PostCreateRequestDto postCreateRequestDto)
    {
        var createdPost = await _postRepository.CreatePost(postCreateRequestDto.CreateDtoToEntity());
        return Result<PostResponseDto>.Success(createdPost.ToDto());
    }

    public async Task<Result<PostResponseDto>> UpdatePost(int postId, PostUpdateRequestDto postUpdateRequestDto)
    {
        var post = await _postRepository.GetPostById(postId);

        if (post == null)
        {
            return Result<PostResponseDto>.Failure("Post does not exist");
        }

        var updatedPost = await _postRepository.UpdatePost(postUpdateRequestDto.UpdateDtoToEntity(post));
        return Result<PostResponseDto>.Success(updatedPost.ToDto());
    }
}
