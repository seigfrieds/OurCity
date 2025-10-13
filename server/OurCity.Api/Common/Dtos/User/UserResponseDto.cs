namespace OurCity.Api.Common.Dtos.User;

public class UserResponseDto
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public string? DisplayName { get; set; }

    public required List<PostResponseDto> Posts { get; set; } = new();

    public required DateTime CreatedAt { get; set; }

    public required DateTime UpdatedAt { get; set; }

    public required bool IsDeleted { get; set; }
}
