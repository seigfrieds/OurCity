namespace OurCity.Api.Common.Dtos.Comments;

public class CommentResponseDto
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int AuthorId { get; set; }
    public required string Content { get; set; }

    public List<int> UpvotedUserIds { get; set; } = new();
    
    public List<int> DownvotedUserIds { get; set; } = new();

    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } 
}
