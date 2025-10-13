using OurCity.Api.Common.Dtos.User;
using OurCity.Api.Infrastructure.Database;

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
            IsDeleted = user.IsDeleted,
        };
    }

    public static User CreateDtoToEntity(this UserCreateRequestDto userCreateRequestDto)
    {
        return new User
        {
            Auth0Id = userCreateRequestDto.Auth0Id,
            Username = userCreateRequestDto.Username,
            Email = userCreateRequestDto.Email,
            DisplayName = userCreateRequestDto.DisplayName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }

    public static User UpdateDtoToEntity(
        this UserUpdateRequestDto userUpdateRequestDto,
        User existingUser
    )
    {
        existingUser.Username = userUpdateRequestDto.Username ?? existingUser.Username;
        existingUser.Email = userUpdateRequestDto.Email ?? existingUser.Email;
        existingUser.DisplayName = userUpdateRequestDto.DisplayName ?? existingUser.DisplayName;
        existingUser.UpdatedAt = DateTime.UtcNow;

        return existingUser;
    }
}
