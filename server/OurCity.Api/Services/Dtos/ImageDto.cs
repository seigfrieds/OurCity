using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Services.Dtos;

public class ImageDto
{
    [Required(ErrorMessage = "Url is required")]
    public required string Url { get; set; }
}
