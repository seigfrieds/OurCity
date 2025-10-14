namespace OurCity.Api.Services.Dtos;

public class PostResponseDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required int Votes { get; set; }

    public required string? Location { get; set; }

    public required List<ImageDto> Images { get; set; } = new();
}
