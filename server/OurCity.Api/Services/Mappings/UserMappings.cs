using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Services.Dtos;

namespace OurCity.Api.Services.Mappings;

public static class UserMappings
{
    public static IEnumerable<UserResponseDto> ToDtos(this IEnumerable<User> users)
    {
        return users.Select(user => user.ToDto());
    }

    public static UserResponseDto ToDto(this User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Posts = user.Posts.Select(post => post.ToDto()).ToList(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsDeleted = user.IsDeleted
        };
    }

    public static User ToEntity(this UserRequestDto userRequestDto)
    {
        return new User
        {
            Auth0Id = userRequestDto.Auth0Id,
            Username = userRequestDto.Username,
            Email = userRequestDto.Email,
            DisplayName = userRequestDto.DisplayName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}