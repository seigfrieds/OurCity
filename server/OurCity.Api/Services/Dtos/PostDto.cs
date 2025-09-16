namespace OurCity.Api.Services.Dtos;

public class PostDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}
