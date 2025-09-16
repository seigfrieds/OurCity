namespace OurCity.Api.Infrastructure.Database;

public class Post
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public List<Comment>? Comments { get; set; }
}
