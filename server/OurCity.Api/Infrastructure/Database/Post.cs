namespace OurCity.Api.Infrastructure.Database;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Location { get; set; }
    public List<int> UpvotedUserIds { get; set; } = new();
    public List<int> DownvotedUserIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<Image> Images { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
}
