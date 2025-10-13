using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Common.Dtos;

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

    public static Post ToEntity(this PostRequestDto postRequestDto)
    {
        return new Post
        {
            Title = postRequestDto.Title,
            Description = postRequestDto.Description,
            Location = postRequestDto.Location,
            Images = postRequestDto
                .Images.Select(imgDto => new Image { Url = imgDto.Url })
                .ToList(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }
}
