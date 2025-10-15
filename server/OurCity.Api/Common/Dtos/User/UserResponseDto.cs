namespace OurCity.Api.Common.Dtos.User;

public class UserResponseDto
{
    public int Id { get; set; }

    public string? DisplayName { get; set; }

    public required List<int> PostIds { get; set; } = new();

    public required DateTime CreatedAt { get; set; }

    public required DateTime UpdatedAt { get; set; }

    public required bool IsDeleted { get; set; }
}
