namespace OurCity.Api.Infrastructure.Database;

public class Comment
{
    public required int Id { get; set; }
    public required string Content { get; set; }

    public required Post Post { get; set; }
}
