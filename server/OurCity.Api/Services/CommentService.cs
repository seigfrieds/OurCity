using OurCity.Api.Common;
using OurCity.Api.Common.Dtos.Comments;
using OurCity.Api.Common.Enum;
using OurCity.Api.Infrastructure;
using OurCity.Api.Services.Mappings;

namespace OurCity.Api.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentResponseDto>> GetCommentsByPostId(int postId);
    Task<CommentResponseDto> GetCommentById(int postId, int commentId);
    Task<Result<CommentResponseDto>> CreateComment(
        int postId,
        CommentCreateRequestDto commentCreateRequestDto
    );
    Task<Result<CommentResponseDto>> UpdateComment(
        int postId,
        int commentId,
        CommentUpdateRequestDto commentUpdateRequestDto
    );
    Task<Result<CommentResponseDto>> VoteComment(
        int postId,
        int commentId,
        int userId,
        VoteType voteType
    );
    Task<Result<CommentResponseDto>> DeleteComment(int postId, int commentId);
}

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<CommentResponseDto>> GetCommentsByPostId(int postId)
    {
        return (await _commentRepository.GetCommentsByPostId(postId)).ToDtos();
    }

    public async Task<CommentResponseDto> GetCommentById(int postId, int commentId)
    {
        var comment = await _commentRepository.GetCommentById(postId, commentId);
        return comment.ToDto();
    }

    public async Task<Result<CommentResponseDto>> CreateComment(
        int postId,
        CommentCreateRequestDto commentCreateRequestDto
    )
    {
        var createdComment = await _commentRepository.CreateComment(
            commentCreateRequestDto.ToEntity(postId)
        );
        return Result<CommentResponseDto>.Success(createdComment.ToDto());
    }

    public async Task<Result<CommentResponseDto>> UpdateComment(
        int postId,
        int commentId,
        CommentUpdateRequestDto commentUpdateRequestDto
    )
    {
        var existingComment = await _commentRepository.GetCommentById(postId, commentId);
        var updatedComment = await _commentRepository.UpdateComment(
            commentUpdateRequestDto.ToEntity(existingComment)
        );
        return Result<CommentResponseDto>.Success(updatedComment.ToDto());
    }

    public async Task<Result<CommentResponseDto>> VoteComment(
        int postId,
        int commentId,
        int userId,
        VoteType voteType
    )
    {
        var comment = await _commentRepository.GetCommentById(postId, commentId);

        if (comment == null)
        {
            return Result<CommentResponseDto>.Failure("Comment does not exist");
        }

        var targetList =
            (voteType == VoteType.Upvote) ? comment.UpvotedUserIds : comment.DownvotedUserIds;
        var oppositeList =
            (voteType == VoteType.Upvote) ? comment.DownvotedUserIds : comment.UpvotedUserIds;

        if (!targetList.Remove(userId))
        {
            targetList.Add(userId);
            oppositeList.Remove(userId);
        }

        comment.UpdatedAt = DateTime.UtcNow;

        var updatedComment = await _commentRepository.UpdateComment(comment);
        return Result<CommentResponseDto>.Success(updatedComment.ToDto());
    }

    public async Task<Result<CommentResponseDto>> DeleteComment(int postId, int commentId)
    {
        var existingComment = await _commentRepository.GetCommentById(postId, commentId);
        if (existingComment == null)
        {
            return Result<CommentResponseDto>.Failure("Comment not found.");
        }

        await _commentRepository.DeleteComment(existingComment);
        return Result<CommentResponseDto>.Success(existingComment.ToDto());
    }
}
