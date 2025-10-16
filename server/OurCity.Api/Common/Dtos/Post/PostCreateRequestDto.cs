using System.ComponentModel.DataAnnotations;
using OurCity.Api.Common.Dtos.Image;

namespace OurCity.Api.Common.Dtos.Post;

public class PostCreateRequestDto
{
    [Required(ErrorMessage = "AuthorId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be a positive integer")]
    public required int AuthorId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
    public required string Title { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    [Required(ErrorMessage = "Description is required")]
    public required string Description { get; set; }

    [StringLength(50, ErrorMessage = "Location cannot exceed 50 characters")]
    public string? Location { get; set; }

    public List<ImageDto> Images { get; set; } = new();
}
