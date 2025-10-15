namespace OurCity.Api.Common.Dtos;

public class PostUpvoteResponseDto
{
    public required int Id { get; set; }
    public required int AuthorId { get; set; }
    public required bool Upvoted { get; set; }
}
