using OurCity.Api.Infrastructure.Database;
using OurCity.Api.Common.Dtos.Comments;

namespace OurCity.Api.Services.Mappings;

public static class CommentMappings
{
    public static IEnumerable<CommentResponseDto> ToDtos(this IEnumerable<Comment> comments)
    {
        return comments.Select(comment => comment.ToDto());
    }

    public static CommentResponseDto ToDto(this Comment comment)
    {
        return new CommentResponseDto
        {
            Id = comment.Id,
            PostId = comment.PostId,
            AuthorId = comment.AuthorId,
            Content = comment.Content,
            UpvotedUserIds = comment.UpvotedUserIds,
            DownvotedUserIds = comment.DownvotedUserIds,
            IsDeleted = comment.IsDeleted,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt
        };
    }

    public static Comment ToEntity(
        this CommentCreateRequestDto commentCreateRequestDto,
        int postId
    )
    {
        return new Comment
        {
            PostId = postId,
            AuthorId = commentCreateRequestDto.AuthorId,
            Content = commentCreateRequestDto.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static Comment ToEntity(
        this CommentUpdateRequestDto commentUpdateRequestDto,
        Comment existingComment
    )
    {
        existingComment.Content = commentUpdateRequestDto.Content ?? existingComment.Content;
        existingComment.UpdatedAt = DateTime.UtcNow;
        return existingComment;
    }
}