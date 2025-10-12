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

    public static Post UpdateDtoToEntity(this PostUpdateRequestDto postUpdateRequestDto, Post existingPost)
    {
        return new Post
        {
            Id = existingPost.Id,
            Title = postUpdateRequestDto.Title ?? existingPost.Title,
            Description = postUpdateRequestDto.Description ?? existingPost.Description,
            Location = postUpdateRequestDto.Location ?? existingPost.Location,
            Images = postUpdateRequestDto.Images.Count != 0
                ? postUpdateRequestDto.Images.Select(imgDto => new Image { Url = imgDto.Url }).ToList()
                : existingPost.Images,
            // Have the number of votes available for update, we will add on role based access control later
            // May have to make a separate method in the services layer for updating votes only
            Votes = postUpdateRequestDto.Votes ?? existingPost.Votes,
            CreatedAt = existingPost.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
        };
    }
}
