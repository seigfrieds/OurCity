using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Services.Dtos;

public class PostResponseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public required string Description { get; set; }
}
