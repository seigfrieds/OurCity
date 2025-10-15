using OurCity.Api.Common.Enum;

namespace OurCity.Api.Common.Dtos;

public class CommentVoteRequestDto
{
    public required int UserId { get; set; }

    public required VoteType VoteType { get; set; }
}
