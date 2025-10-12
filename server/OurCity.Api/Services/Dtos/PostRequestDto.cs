using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Services.Dtos;

public class PostRequestDto
{
    [Required(ErrorMessage = "Title is required")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public required string Description { get; set; }

    public string? Location { get; set; }

    public List<ImageDto> Images { get; set; } = new();
}
