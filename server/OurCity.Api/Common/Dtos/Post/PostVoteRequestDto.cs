using OurCity.Api.Common.Enum;

namespace OurCity.Api.Common.Dtos.Post;

public class PostVoteRequestDto
{
    public required int UserId { get; set; }

    public required VoteType VoteType { get; set; }
}
