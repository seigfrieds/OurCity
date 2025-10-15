using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Common.Dtos.Comments;

public class CommentUpdateRequestDto
{
    [Required(ErrorMessage = "Content is required")]
    [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters")]
    public required string Content { get; set; }
}
