namespace OurCity.Api.Common.Dtos;

public class PostDownvoteResponseDto
{
    public required int Id { get; set; }
    public required int AuthorId { get; set; }
    public required bool Downvoted { get; set; }
}