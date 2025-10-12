using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Services.Dtos;

namespace OurCity.Api.Services.Mappings;

public static class PostMappings
{
    public static IEnumerable<PostResponseDto> ToDtos(this IEnumerable<Post> posts)
    {
        return posts.Select(post => post.ToDto());
    }

    public static PostResponseDto ToDto(this Post post)
    {
        return new PostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Votes = post.Votes,
            Location = post.Location,
            Images = post.Images.Select(image => new ImageDto { Url = image.Url }).ToList(),
        };
    }

    public static Post CreateDtoToEntity(this PostCreateRequestDto postCreateRequestDto)
    {
        return new Post
        {
            Title = postCreateRequestDto.Title,
            Description = postCreateRequestDto.Description,
            Location = postCreateRequestDto.Location,
            Images = postCreateRequestDto
                .Images.Select(imgDto => new Image { Url = imgDto.Url })
                .ToList(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }
}
