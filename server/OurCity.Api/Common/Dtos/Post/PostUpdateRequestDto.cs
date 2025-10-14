using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Common.Dtos;

public class PostUpdateRequestDto
{
    [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
    public string? Title { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [StringLength(50, ErrorMessage = "Location cannot exceed 50 characters")]
    public string? Location { get; set; }

    public List<ImageDto> Images { get; set; } = new();
}
