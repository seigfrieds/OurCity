using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Services.Dtos;

namespace OurCity.Api.Services.Mappings;

public static class PostMappings
{
    public static IEnumerable<PostDto> ToDtos(this IEnumerable<Post> posts)
    {
        return posts.Select(post => post.ToDto());
    }

    public static PostDto ToDto(this Post post)
    {
        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
        };
    }
}
