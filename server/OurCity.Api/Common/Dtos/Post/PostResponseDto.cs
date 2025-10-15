namespace OurCity.Api.Common.Dtos.Post;

public class PostResponseDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Votes { get; set; }
    public required string? Location { get; set; }
    public List<int> CommentIds { get; set; } = new();
}
