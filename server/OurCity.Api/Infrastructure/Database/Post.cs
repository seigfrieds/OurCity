namespace OurCity.Api.Infrastructure.Database;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Votes { get; set; } = 0;
    public string Location { get; set; } = "";
    public List<string>? ImageUrls { get; set; } = new();
    public List<Comment>? Comments { get; set; } = new();
}
