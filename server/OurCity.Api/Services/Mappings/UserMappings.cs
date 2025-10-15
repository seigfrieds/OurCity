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
            DisplayName = user.DisplayName,
            PostIds = user.Posts.Select(p => p.Id).ToList(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsDeleted = user.IsDeleted,
        };
    }

    public static User CreateDtoToEntity(this UserCreateRequestDto userCreateRequestDto)
    {
        return new User
        {
            Username = userCreateRequestDto.Username,
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
        existingUser.Username = userUpdateRequestDto.Username;
        existingUser.DisplayName = userUpdateRequestDto.DisplayName ?? existingUser.DisplayName;
        existingUser.UpdatedAt = DateTime.UtcNow;

        return existingUser;
    }
}
