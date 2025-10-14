namespace OurCity.Api.Common.Dtos;

public class PostResponseDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public List<int> UpvotedUserIds { get; set; } = new();
    public List<int> DownvotedUserIds { get; set; } = new();

    public required string? Location { get; set; }

    public required List<ImageDto> Images { get; set; } = new();
}
