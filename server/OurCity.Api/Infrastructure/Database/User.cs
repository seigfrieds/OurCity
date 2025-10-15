namespace OurCity.Api.Infrastructure.Database;

public class User
{
    public int Id { get; set; }

    public string? DisplayName { get; set; }

    public List<Post> Posts { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
}
