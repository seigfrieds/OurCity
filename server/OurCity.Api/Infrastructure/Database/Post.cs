namespace OurCity.Api.Infrastructure.Database;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }

    public List<Comment>? Comments { get; set; }
}
